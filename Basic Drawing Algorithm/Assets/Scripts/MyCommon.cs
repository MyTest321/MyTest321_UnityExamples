using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MyCommon
{
    public static void Swap<T>(ref T a, ref T b)
    {
        T tmp = a;
        a = b;
        b = tmp;
    }

    public static void DrawPoint(Texture2D tex, int x, int y, Color c, int ptSize = 1)
    {
		ptSize = Mathf.Max(0, ptSize);
		if (ptSize == 0) return;
		if (ptSize == 1)
		{
			tex.SetPixel(x, y, c);
			tex.SetPixel(x - 1, y, c);
			tex.SetPixel(x + 1, y, c);
			tex.SetPixel(x, y + 1, c);
			tex.SetPixel(x, y - 1, c);
			return;
		}

		int stX = x - ptSize;
		int stY = y - ptSize;

		int edX = x + ptSize;
		int edY = y + ptSize;

		for (int _x = stX; _x < edX && _x < tex.width; ++_x) {
			for (int _y = stY; _y < edY && _y < tex.height; ++_y) {
				tex.SetPixel(_x, _y, c);
			}
		}
    }

    // http://www.songho.ca/opengl/gl_projectionmatrix.html
    public static Matrix4x4 GetProjectMatrix(float left, float right, float bottom, float top, float near, float far)
    {
        Matrix4x4 m = new Matrix4x4();
        m[0] = (2 * near) / (right - left);
        m[5] = (2 * near) / (top - bottom);
        m[8] = (right + left) / (right - left);
        m[9] = (top + bottom) / (top - bottom);
        m[10] = -(far + near) / (far - near);
        m[11] = -1;
        m[14] = -(2 * far * near) / (far - near);
        return m;
    }

    public static Matrix4x4 GetProjectMatrix(float fov, float aspect, float near, float far)
    {
        var tan = Mathf.Tan(fov / 2 * Mathf.Deg2Rad);
        var h = tan * near;
        var w = h * aspect;
        return GetProjectMatrix(-w, w, -h, h, near, far);
    }

    // http://www.songho.ca/opengl/gl_camera.html
    public static Matrix4x4 GetViewMatrix(Camera camera)
    {
        var m = new Matrix4x4();
        var transform = camera.transform;
        var e = transform.position;
        var f = -transform.forward;
        var u = transform.up;
        var l = Vector3.Cross(f, u); // Unity left hand: f->u
        // Mview = mul(Mrotation, Mtranslate)
/*
        Mrotation     =    [ lx ux fx 0 ]^(-1)
                           [ ly uy fy 0 ]
                           [ lz uz fz 0 ]
                           [ 0  0  0  1 ]

                      =    [ lx ux fx 0 ]^T
                           [ ly uy fy 0 ]
                           [ lz uz fz 0 ]
                           [ 0  0  0  1 ]

                      =    [ lx ly lz 0 ]
                           [ ux uy uz 0 ]
                           [ fx fy fz 0 ]
                           [ 0  0  0  1 ]

        Mtranslate    =    [ 1  0  0 -ex]
                           [ 0  1  0 -ey]
                           [ 0  0  1 -ez]
                           [ 0  0  0  1 ]

        Mview         =    [ lx ly lz (-ex*lx -ey*ly -ez*lz) ]
                           [ ux uy uz (-ex*ux -ey*uy -ez*uz) ]
                           [ fx fy fz (-ex*fx -ey*fy -ez*fz) ]
                           [ 0  0  0            1            ]
*/
        m.SetRow(0, l);
        m.SetRow(1, u);
        m.SetRow(2, f);
        m[12] = -e.x * l.x - e.y * l.y - e.z * l.z;
        m[13] = -e.x * u.x - e.y * u.y - e.z * u.z;
        m[14] = -e.x * f.x - e.y * f.y - e.z * f.z;
        m[15] = 1;
        return m;
    }

    public static Matrix4x4 GetModelMatrix(Transform transform)
    {
        var p = transform.position;
        var q = transform.rotation;
        var s = transform.localScale;
/*
        Mmodel     =  Mtranslation * Mrotation * Mscale
        Mscale     =   [ sx 0  0  0  ]
                       [ 0  sy 0  0  ]
                       [ 0  0  sz 0  ]
                       [ 0  0  0  1  ]

        Mtranslate =   [ 1  0  0  px ]
                       [ 0  1  0  py ]
                       [ 0  0  1  pz ]
                       [ 0  0  0  1  ]

        Mrotation  = q is Quaternion, q.eulerAnlges is kind of guess what rx should be in Euler, so it's inaccurate (floating point problem)

        var rx = RotateXMatrix(q.eulerAngles.x);
        var ry = RotateYMatrix(q.eulerAngles.y);
        var rz = RotateZMatrix(q.eulerAngles.z);
        var Mrotation1 = rz * ry * rx;   // Unity order -> zyx

        var Mrz = GetRotateMatrix(transform.forward, q.eulerAngles.z);
        var Mry = GetRotateMatrix(transform.up, q.eulerAngles.y);
        var Mrx = GetRotateMatrix(transform.right, q.eulerAngles.x); // Mrx: transform.right(localspace x axis) with q.eulerAngles.x(rotate angle) -> (x degrees around the x axis)
        var Mrotation2 = Mrz * Mry * Mrx;

        compare to Unity Rotate matrix -> 'Mrotation1' and 'Mrotation2' and 'Mrotation3'
        var ex  = Matrix4x4.Rotate(Quaternion.Euler(q.eulerAngles.x, 0, 0));
        var ey  = Matrix4x4.Rotate(Quaternion.Euler(0, q.eulerAngles.y, 0));
        var ez  = Matrix4x4.Rotate(Quaternion.Euler(0, 0, q.eulerAngles.z));
        var Mrotation3 = ez * ey * ex;
*/
        var Mscale = Matrix4x4.identity;
        Mscale[0] = s.x;
        Mscale[5] = s.y;
        Mscale[10] = s.z;
        Mscale[15] = 1;
        var Mrotation = GetRotateMatrix(q);
        var Mtranslate = Matrix4x4.identity;
        Mtranslate.SetColumn(3, new Vector4(p.x, p.y, p.z, 1));
        return Mtranslate * Mrotation * Mscale; // equals to return Matrix4x4.TRS(p, q, s);
    }

    // Quaternion-derived rotation matrix: https://en.wikipedia.org/wiki/Quaternions_and_spatial_rotation#Quaternion-derived_rotation_matrix
    // Equals to Matrix4x4.Rotate(Quaternion q)
    public static Matrix4x4 GetRotateMatrix(Quaternion q)
    {
        float x2 = q.x * q.x;
        float y2 = q.y * q.y;
        float z2 = q.z * q.z;
        float xy = q.x * q.y;
        float xz = q.x * q.z;
        float yz = q.y * q.z;
        float wx = q.w * q.x;
        float wy = q.w * q.y;
        float wz = q.w * q.z;

        // This calculation would be a lot more complicated for non-unit length quaternions
        // Note: The constructor of Matrix4x4 expects the Matrix in column-major format like expected by
        // OpenGL
        return new Matrix4x4(
            new Vector4(1 - 2 * (y2 + z2), 2 * (xy + wz),     2 * (xz - wy),     0),  // Column0
            new Vector4(2 * (xy - wz),     1 - 2 * (x2 + z2), 2 * (yz + wx),     0),  // Column1
            new Vector4(2 * (xz + wy),     2 * (yz - wx),     1 - 2 * (x2 + y2), 0),  // Column2
            new Vector4(0,                 0,                 0,                 1)   // Column3
        );
    }

    // Rodrigues's formula: http://www.songho.ca/opengl/gl_rotate.html
    public static Matrix4x4 GetRotateMatrix(Vector3 axis, float angle)
    {
        var m = new Matrix4x4();
        float c = Mathf.Cos(angle * Mathf.Deg2Rad);
        float s = Mathf.Sin(angle * Mathf.Deg2Rad);
        float x = axis.x;
        float y = axis.y;
        float z = axis.z;
        float c1 = 1 - c;
        float xx = x * x;
        float yy = y * y;
        float zz = z * z;
        float xy = x * y;
        float xz = x * z;
        float yz = y * z;
        float sx = s * x;
        float sy = s * y;
        float sz = s * z;

        m[0] = c1 * xx + c;
        m[1] = c1 * xy + sz;
        m[2] = c1 * xz - sy;
        m[4] = c1 * xy - sz;
        m[5] = c1 * yy + c;
        m[6] = c1 * yz + sx;
        m[8] = c1 * xz + sy;
        m[9] = c1 * yz - sx;
        m[10] = c1 * zz + c;
        m[15] = 1;
        return m;
    }

    // http://www.songho.ca/opengl/gl_anglestoaxes.html
    public static Matrix4x4 RotateXMatrix(float angle)
    {
        Matrix4x4 m = Matrix4x4.identity;
        float c = Mathf.Cos(angle * Mathf.Deg2Rad);
        float s = Mathf.Sin(angle * Mathf.Deg2Rad);
        m[5]  =  c;
        m[6]  =  s;
        m[9]  = -s;
        m[10] =  c;
        return m;
    }

    public static Matrix4x4 RotateYMatrix(float angle)
    {
        Matrix4x4 m = Matrix4x4.identity;
        float c = Mathf.Cos(angle * Mathf.Deg2Rad);
        float s = Mathf.Sin(angle * Mathf.Deg2Rad);
        m[0]  =  c;
        m[2]  = -s; // m[2] = -s, m[8] = s. something wired in Unity coordinate.
        m[8]  =  s;
        m[10] =  c;
        return m;
    }

    public static Matrix4x4 RotateZMatrix(float angle)
    {
        Matrix4x4 m = Matrix4x4.identity;
        float c = Mathf.Cos(angle * Mathf.Deg2Rad);
        float s = Mathf.Sin(angle * Mathf.Deg2Rad);
        m[0] =  c;
        m[1] =  s;
        m[4] = -s;
        m[5] =  c;
        return m;
    }
}
