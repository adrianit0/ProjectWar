using UnityEngine;

public struct IntVector2 {

    public int x;
    public int y;

    public IntVector2 (int _x, int _y) {
        x = _x;
        y = _y;
    }

    static public implicit operator IntVector2 (Vector2 vector) {
        return new IntVector2(Mathf.RoundToInt(vector.x), Mathf.RoundToInt(vector.y));
    }

    static public implicit operator IntVector2(Vector3 vector) {
        return new IntVector2(Mathf.RoundToInt(vector.x), Mathf.RoundToInt(vector.y));
    }

    static public explicit operator Vector2 (IntVector2 intVector) {
        return new Vector2 (intVector.x, intVector.y);
    }

    static public explicit operator Vector3(IntVector2 intVector) {
        return new Vector3(intVector.x, intVector.y, 0);
    }
}
