using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Vec3 {
    float x, y, z;

    public Vec3(float _x, float _y, float _z) {
        x = _x;
        y = _y;
        z = _z;
    }

    public Vec3(Vector3 vec3) {
        x = vec3.x;
        y = vec3.z;
        z = vec3.y;
    }

    public Vector3 unity() {
        return new Vector3(x, y, z);
    }

}

public static class math {
    static void set_transform_x(ref Transform trans, float val) {
        var vec = trans.position;
        trans.position = new Vector3(val, vec.y, vec.z);
    }

    static void set_transform_y(ref Transform trans, float val) {
        var vec = trans.position;
        trans.position = new Vector3(val, vec.y, vec.z);
    }

    static void set_transform_z(ref Transform trans, float val) {
        var vec = trans.position;
        trans.position = new Vector3(val, vec.y, vec.z);
    }

    static Vector2 rotate_vector2(Vector2 vec, float degrees) {
        float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
        float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);
        float tx = vec.x;
        float ty = vec.y;

        vec.x = (cos * tx) - (sin * ty);
        vec.y = (sin * tx) + (cos * ty);
        return vec;
    }

    static Vector3 rotate_vector3(Vector3 vec, float degrees) {
        float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
        float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);
        float tx = vec.x;
        float tz = vec.z;

        vec.x = (cos * tx) - (sin * tz);
        vec.z = (sin * tx) + (cos * tz);
        return vec;
    }
}

