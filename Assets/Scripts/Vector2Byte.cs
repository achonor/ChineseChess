using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts {
    public class Vector2Byte {
        public sbyte x;
        public sbyte y;

        public Vector2Byte(sbyte _x, sbyte _y) {
            x = _x;
            y = _y;
        }

        public Vector2Byte(int _x, int _y) {
            x = (sbyte)_x;
            y = (sbyte)_y;
        }


        public bool IsEqules(Vector2Byte vector) {
            return x == vector.x && y == vector.y;
        }

        public static Vector2Byte operator+ (Vector2Byte a, Vector2Byte b) {
            return new Vector2Byte((sbyte)(a.x + b.x), (sbyte)(a.y + b.y));
        }
        public static Vector2Byte operator /(Vector2Byte a, sbyte b) {
            return new Vector2Byte((sbyte)(a.x / b), (sbyte)(a.y / b));
        }
    }
}
