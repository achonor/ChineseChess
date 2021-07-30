using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts {
    public class Vector2Byte {
        public byte x;
        public byte y;

        public Vector2Byte(byte _x, byte _y) {
            x = _x;
            y = _y;
        }

        public static Vector2Byte operator+ (Vector2Byte a, Vector2Byte b) {
            return new Vector2Byte((byte)(a.x + b.x), (byte)(a.y + b.y));
        }
    }
}
