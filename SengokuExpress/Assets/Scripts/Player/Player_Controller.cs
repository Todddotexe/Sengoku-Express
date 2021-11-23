/// Author: Matin Kamali
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Character_Util; 

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(CharacterController))]
// [RequireComponent(typeof(Animator))]
public class Player_Controller : MonoBehaviour {
	
    // !=================================================================
    // !======================= Player Controller =======================
    // !=================================================================
    public ParticleSystem part_dash; // TODO add this back when we have a proper particle system
    public ParticleSystem part_bark; // TODO add this back when we have a proper particle system

    /// ===
    /// FIELDS
    /// ===
    Transform transf = null; // cached transform
    [SerializeField] Movement movement = new Movement();   // ! the reason we initialise these here is because I'm testing whether we can change the values in Unity Editor and keep it. If we initialise them in Start(), all the changes in UnityEditor will be lost. Especially if there's an asset field like the Dash_Particle alex tried to put in.
    [SerializeField] Dash dash = new Dash();
    [SerializeField] Player_Bark bark = new Player_Bark();
    [SerializeField] Player_Binding binds = new Player_Binding();
    [SerializeField] Dog_Audio audio_source = new Dog_Audio();
    [SerializeField] Combat combat = new Combat();
    [SerializeField] Player_Components components = new Player_Components();
    Player_Inputs inputs = new Player_Inputs();
    bool has_hit_enemy = false;
    public Vector3 attack_hitbox_offset;
    public Vector3 attack_hitbox_extents;
    public bool god_mode = false;

    [SerializeField] float dash_cool_down_duration = 0.5f;
    float dash_cool_down = 0f;
    float timer_knockback_init = 0.1f;
    float timer_knockback = 0.1f;
    float bark_meter_percentage = 0f; // goes from 0 - 1
    const float BARK_METER_POINT = 0.1f;

    /// initialise fields
    void Start() {
        // -- setup fields
        transf = transform;
        bark_meter_percentage = 0;
        Global.set_bark_meter(bark_meter_percentage);
        Global.set_health(health);
        // -- setup components
        components.input                = GetComponent<PlayerInput>();
        components.character_controller = GetComponent<CharacterController>();
        // components.animator             = GetComponent<Animator>(); // ! this is now being set through the editor
        // components.main_audio_source         = GetComponent<AudioSource>(); // ! this is now meant to be set through the editor
        Debug.Assert(components.main_audio_source != null);
        Debug.Assert(components.walk_audio_source != null);
        components.walk_audio_source.clip = audio_source.walk;
        components.walk_audio_source.loop = true;
        play_walk_audio(false);
        // -- setup inputs
        Debug.Assert(components.input.actions != null);
        inputs.walk       = components.input.actions[binds.WALK_INPUT_LABEL];
        inputs.dash       = components.input.actions[binds.DASH_INPUT_LABEL];
        inputs.bark       = components.input.actions[binds.BARK_INPUT_LABEL];
        inputs.attack     = components.input.actions[binds.ATTACK_INPUT_LABEL];
        inputs.dash.performed      -= delegate_dash;
        inputs.bark.performed      -= delegate_bark;
        inputs.attack.performed    -= delegate_attack;
        
        inputs.dash.performed += delegate_dash;
        inputs.bark.performed += delegate_bark;
        inputs.attack.performed += delegate_attack;
        // -- setup attack combo chain
        combat.attack_functions_start.Add(delegate_attack_1_start);
        combat.attack_functions_start.Add(delegate_attack_2_start);
        combat.attack_functions_start.Add(delegate_attack_3_start);
        combat.attack_functions_update.Add(delegate_attack_1_update);
        combat.attack_functions_update.Add(delegate_attack_2_update);
        combat.attack_functions_update.Add(delegate_attack_3_update);
    }
    /// Update to read input values
    void Update() {
        // -- update input
        var input_vec2    = inputs.walk.ReadValue<Vector2>();
        inputs.input_vec2 = input_vec2;
        inputs.input.x    = input_vec2.x;
        inputs.input.y    = 0;
        inputs.input.z    = input_vec2.y;
    }
    /// physics update
    void FixedUpdate() {
        if (Global.get_state() == Global.STATES.PAUSED) return; // don't do anything
        if (!is_alive) {
            Destroy(gameObject);
            Global.set_game_state(Global.STATES.LOST);
            return;
        }
        { // -- dash cool down
            if (dash_cool_down > 0) dash_cool_down -= Time.deltaTime;
        }
        if (is_knocked_back) {
            if (timer_knockback > 0) {
                timer_knockback -= Time.deltaTime;
                PLAYER_apply_dash();
                PLAYER_apply_velocity();
                return;
            } else {
                timer_knockback = timer_knockback_init;
                is_knocked_back = false;
            }
        }
        
        if (combat.is_attacking || combat.queued_combo) {
            combat.update();
        } else
        if (dash.is_in_progress) {
            // -- if dash is in progress, apply and update dash.
            PLAYER_apply_dash();
        } else
        if (inputs.input_vec2.magnitude > 0) {
            play_walk_audio(true);
            components.animator.SetBool(binds.ANIMATION_BOOL_WALK, true);
            movement.velocity = inputs.input * movement.speed * Time.deltaTime;
        } else {
            play_walk_audio(false);
            components.animator.SetBool(binds.ANIMATION_BOOL_WALK, false);
        }
        PLAYER_apply_velocity();
    }
    /// hit this enemy
    bool is_hit = false;
    bool is_alive = true;
    bool is_knocked_back = false;
    int health = 3;
    public void hit(int damage) {
        is_hit = true;
        // -- start hit animation
        // -- apply damage
        if (!god_mode) {
            health -= damage;
            Global.set_health(health);
            if (health <= 0) {
                is_alive = false;
            }
        }
    }
    /// knock back and stun enemy
    public void knock_back(Vector2 direction) {
        dash.dash(transf.position, direction, Dash.TYPES.KNOCKBACK);
        is_knocked_back = true;
        //is_stunned = true;
    }
    /// draw debugging aids
    void OnDrawGizmos() {
        // -- bark area
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, bark.radius);
        // -- dash range
        Vector3 dash_offset = new Vector3(0, 1, 0) + transform.position;
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(dash_offset, dash_offset + transform.forward * dash.normal_dash_range);
        // -- attack hitbox
        Gizmos.DrawWireCube(transform.position + (attack_hitbox_offset).rotate(transform.rotation.eulerAngles.y), attack_hitbox_extents);
    }
    /// used as a delegate for Player_Inputs.dash
    void delegate_dash(InputAction.CallbackContext obj) {
        if (Global.get_state() == Global.STATES.PAUSED) return; // don't do anything
        if (dash_cool_down <= 0) {
            dash_cool_down = dash_cool_down_duration;
            dash.dash(transf.position, inputs.input_vec2, Dash.TYPES.NORMAL);
            components.animator.SetTrigger(binds.ANIMATION_TRIGGER_DASH);
            
            play_audio(audio_source.dash);
            // if (dash_part != null) { // TODO add this back when we have a proper particle system
            //     dash_part.Play();
            // }
            if (part_dash != null) {
                part_dash.Play();
            }
        }
    }
    /// used as a delegate for Player_Inputs.bark
    void delegate_bark(InputAction.CallbackContext obj) {
        if (Global.get_state() == Global.STATES.PAUSED) return; // don't do anything
        if (combat.is_attacking || combat.queued_combo) return; // ! don't bark while attacking
        if (bark_meter_percentage < 1) return; // ! don't bark if bark meter is not filled
        bark_meter_percentage = 0;
        Global.set_bark_meter(bark_meter_percentage);
        components.animator.SetTrigger(binds.ANIMATION_TRIGGER_BARK);
        play_audio(audio_source.bark);
        if (part_bark != null) {
            part_bark.Play();
        }
        // -- init collision
        Collider[] colliders = Physics.OverlapSphere(transf.position, bark.radius);
        foreach (Collider collider in colliders) {
            Enemy_Controller enemy = collider.gameObject.GetComponent<Enemy_Controller>();
            if (enemy != null) {
                enemy.stun();
                print("stunned enemy");
            }
        }
    }
    /// used to queue attack
    void delegate_attack(InputAction.CallbackContext obj) {
        if (Global.get_state() == Global.STATES.PAUSED) return; // don't do anything
        combat.queued_combo = true;
    }
    /// first attack in the combo chain (Start)
    void delegate_attack_1_start() {
        print("attack 1 start");
        has_hit_enemy = false;
        components.animator.SetTrigger(binds.ANIMATION_TRIGGER_ATTACK_1);
        play_audio(audio_source.attack_1);
    }
    /// first attack in the combo chain (Update)
    void delegate_attack_1_update() {
        attack_hit(1);
    }
    /// second attack in the combo chain (Start)
    void delegate_attack_2_start() {
        has_hit_enemy = false;
        print("attack 2 start");
        components.animator.SetTrigger(binds.ANIMATION_TRIGGER_ATTACK_2);
        play_audio(audio_source.attack_2);
    }
    /// second attack in the combo chain (Update)
    void delegate_attack_2_update() {
        attack_hit(2);
    }
    /// third attack in the combo chain (Start)
    void delegate_attack_3_start() {
        has_hit_enemy = false;
        var rot = transf.forward;
        dash.dash(transf.position, new Vector2(rot.x, rot.z), Dash.TYPES.COMBAT);
        components.animator.SetTrigger(binds.ANIMATION_TRIGGER_ATTACK_3);
        play_audio(audio_source.attack_3);
    }
    /// third attack in the combo chain (Update)
    void delegate_attack_3_update() {
        PLAYER_apply_dash();
        attack_hit(3);
    }
    /// this is called when the attack hits an enemy
    void attack_hit(uint attack_combo_index) {
        if (!has_hit_enemy) {
            Collider[] colliders = Physics.OverlapBox(transf.position + (attack_hitbox_offset).rotate(transf.rotation.eulerAngles.y), attack_hitbox_extents, transf.rotation);
            foreach (Collider collider in colliders) {
                Enemy_Controller enemy = collider.gameObject.GetComponent<Enemy_Controller>();
                if (enemy != null) {
                    // -- apply damage
                    enemy.hit(1);
                    bark_meter_percentage += BARK_METER_POINT;
                    Global.set_bark_meter(bark_meter_percentage);
                    {   // -- apply knockback
                        var knock_back_direction = enemy.transform.position - transf.position;
                        enemy.knock_back(new Vector2(knock_back_direction.x, knock_back_direction.z));
                    }
                    {   // -- toggle has hit enemy so we don't hit more enemies or hit the same enemy multiple times
                        print("hit enemy attack combo index: " + attack_combo_index.ToString());
                        has_hit_enemy = true;
                        break;
                    }
                }
            }
        }
    }
    // !=================================================================
    // !=======================    BEHAVIOURS     =======================
    // !=================================================================
    // == AI TREE FUNCTIONS == //
    /// applies dash to velocity. Returns true if the dash it in progress. 
    /// Returns false if dash is completed or the player is no longer dashing (dash.is_dashing() == true)
    void PLAYER_apply_dash() {
        if (dash.is_in_progress) {
            movement.velocity = dash.update(movement.velocity, transf);
        }
    }
    /// apply velocity to the player. Returns true at all times
    void PLAYER_apply_velocity() {
        // -- update face dir to point at input direction not velocity
        Vector3 face_dir = inputs.input;
        transf.forward = Vector3.Slerp(transf.forward, face_dir, movement.rotation_speed * Time.deltaTime);
        // -- friction
        if (movement.velocity.x != 0 || movement.velocity.y != 0) {
            movement.velocity.x = Mathf.Lerp(movement.velocity.x, 0, 
                                            movement.friction * Time.deltaTime);
            movement.velocity.z = Mathf.Lerp(movement.velocity.z, 0, 
                                            movement.friction * Time.deltaTime);
        }
        // -- gravity
        if (!components.character_controller.isGrounded) {
            movement.velocity.y -= movement.gravity * Time.deltaTime;
        }
        // -- apply friction
        components.character_controller.Move(movement.velocity);
    }
    void play_audio(AudioClip clip) {
        components.main_audio_source.clip = clip;
        components.main_audio_source.Play();
    }
    void play_walk_audio(bool value) {
        components.walk_audio_source.enabled = value;
    }
    // !=================================================================
    // !=======================  PRIVATE CLASSES  =======================
    // !=================================================================
    // ! the following private classes are used for grouping variables
    /// Player Bark
    [System.Serializable]
    private class Player_Bark {
        public float radius = 3f;  // default value subject to change through the editor
    }
    /// Binding with Unity Editor Animation and Input system
    [System.Serializable]
    private class Player_Binding {
        public string WALK_INPUT_LABEL           = "Move";
        public string DASH_INPUT_LABEL           = "Dash";
        public string BARK_INPUT_LABEL           = "Bark";
        public string ATTACK_INPUT_LABEL         = "LightAttack";
        public string ANIMATION_TRIGGER_ATTACK_1 = "Attack1";
        public string ANIMATION_TRIGGER_ATTACK_2 = "Attack2";
        public string ANIMATION_TRIGGER_ATTACK_3 = "Attack3";
        public string ANIMATION_TRIGGER_BARK     = "Bark";
        public string ANIMATION_TRIGGER_DASH     = "Dash";
        public string ANIMATION_BOOL_WALK        = "Walk";
    }
    /// Player Inputs
    private class Player_Inputs{
        public Vector3 input = new Vector3();
        public Vector2 input_vec2 = new Vector2();
        public InputAction walk = null;
        public InputAction dash = null;
        public InputAction bark = null;
        public InputAction attack = null;
    }
    /// Player Components
    [System.Serializable]
    private class Player_Components {
        [HideInInspector] public PlayerInput input = null;
        [HideInInspector] public CharacterController character_controller = null;
        public Animator animator = null; // ! this is now required to be set through the editor because the animator is on a child object
        public AudioSource main_audio_source = null;
        public AudioSource walk_audio_source = null;
    }
}
