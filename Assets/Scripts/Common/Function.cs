using System;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

namespace Achonor
{
    public static class Function
    {
        //1970
        private static DateTime time1970 = new DateTime(1970, 1, 1, 0, 0, 0, 0);

        /// <summary>
        /// 服务器和本地时间差(毫秒)
        /// </summary>
        private static long offsetTime;

        /// <summary>
        /// 获取本地时间戳(毫秒)
        /// </summary>
        /// <returns></returns>
        public static long GetLocaLTime() {
            return Convert.ToInt64((DateTime.UtcNow - time1970).TotalMilliseconds);
        }

        /// <summary>
        /// 获取服务器时间戳(毫秒)
        /// </summary>
        /// <returns></returns>
        public static long GetServerTime() {
            return GetLocaLTime() + offsetTime;
        }

        public static void SetServerTime(long serverTime) {
            long oldOffsetTime = offsetTime;
            offsetTime = serverTime - GetLocaLTime();
        }

        public static string DateTime2String(DateTime dateTime) {
            return dateTime.ToString();
        }

        public static DateTime String2DateTime(string timeString) {
            return Convert.ToDateTime(timeString);
        }


        public static byte[] Serialization(byte[] data) {
            //数据长度
            int protoLen = data.Length;
            byte[] head = protoLen.ToByte4();
            //拼接
            return MergeArray<byte>(head, data);
        }

        public static T[] MergeArray<T>(T[] first, T[] second) {
            T[] result = new T[first.Length + second.Length];
            first.CopyTo(result, 0);
            second.CopyTo(result, first.Length);
            return result;
        }


        public static string ToString<T>(this T[] sources, string split) {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < sources.Length; i++) {
                if (0 != i) {
                    result.Append(split);
                }
                result.Append(sources[i].ToString());
            }
            return result.ToString();
        }

        public static void CopyArray<T>(T[] source, int start1, T[] target, int start2, int len) {
            for (int i = 0; i < len; i++) {
                target[start2 + i] = source[start1 + i];
            }
        }

        public static long ToLong(this byte[] bytes) {
            return ToLong(bytes, 0, bytes.Length);
        }

        public static long ToLong(this byte[] bytes, int offset, int len) {
            long result = 0;
            for (int i = 0; i < len; i++) {
                result = result | (((long)bytes[i + offset]) << ((len - i - 1) * 8));
            }
            return result;
        }

        public static int ToInt(this byte[] bytes) {
            return ToInt(bytes, 0, bytes.Length);
        }

        public static int ToInt(this byte[] bytes, int offset, int len) {
            int result = 0;
            for (int i = 0; i < len; i++) {
                result = result | (((int)bytes[i + offset]) << ((len - i - 1) * 8));
            }
            return result;
        }

        public static byte[] ToByte2(this short num) {
            byte[] result = new byte[2];
            for (int i = 1; i >= 0; i--) {
                result[1 - i] = (byte)(255 & (num >> (8 * i)));
            }
            return result;
        }

        public static byte[] ToByte4(this int num) {
            byte[] result = new byte[4];
            for (int i = 3; i >= 0; i--) {
                result[3 - i] = (byte)(255 & (num >> (8 * i)));
            }
            return result;
        }

        public static byte[] ToBytes(this string str) {
           return Encoding.UTF8.GetBytes(str);
        }

        public static bool IsNumber(this string str) {
            int number;
            return int.TryParse(str, out number);
        }

        public static double GetRange(this double value, double min, double max) {
            if (0 != min) {
                value = value > min ? value : min;
            }
            if (0 != max) {
                value = value > max ? max : value;
            }
            return value;
        }

        public static double GetLoop(this double value, double min, double max) {
            while (value > max) {
                value -= max - min;
            }
            while (value < min) {
                value += max - min;
            }
            return value;
        }


        public static List<T> ToList<T>(this LinkedList<T> linkList) {
            List<T> result = new List<T>();
            foreach (T value in linkList) {
                result.Add(value);
            }
            return result;
        }

        /// <summary>
        /// 写入数据到文件
        /// </summary>
        /// <param name="content"></param>
        /// <param name="filePath"></param>
        public static void WriteFile(byte[] content, string filePath) {
            //判断路径是否存在
            FileInfo fileInfo = new FileInfo(filePath);
            if (!fileInfo.Directory.Exists) {
                //不能存在要先创建目录
                fileInfo.Directory.Create();
            }
            //文件已存在，先删除
            if (File.Exists(filePath)) {
                File.Delete(filePath);
            }
            FileStream fileStream = File.Create(filePath);
            fileStream.Write(content, 0, content.Length);
            //保存
            fileStream.Flush();
            fileStream.Close();
        }
        /// <summary>
        /// 获取文件MD5
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetMD5HashFromFile(string fileName) {
            try {
                FileStream file = new FileStream(fileName, FileMode.Open);
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(file);
                file.Close();

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++) {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            }
            catch (Exception ex) {
                throw new Exception("GetMD5HashFromFile() fail,error:" + ex.Message);
            }
        }

        public static string CreateiBeaconAdvertiseData(string uuid, ushort major, ushort minor, byte txPower = 0xC5) {
            string beaconType = "0215";
            uuid = uuid.Replace("-", string.Empty);
            string majorStr = major.ToString("X").PadLeft(4, '0');
            string minorStr = minor.ToString("X").PadLeft(4, '0');
            string txPowerStr = txPower.ToString("X").PadLeft(2, '0');
            return beaconType + uuid + majorStr + minorStr + txPowerStr;
        }

        public static bool Vector3Compare(Vector3 v1, Vector3 v2, float differ = 0.000001f) {
            return (Mathf.Abs(v1.x - v2.x) <= differ) && (Mathf.Abs(v1.y - v2.y) <= differ) && (Mathf.Abs(v1.z - v2.z) <= differ);
        }

        public static List<float> String2ListFloat(string str) {
            List<float> retList = new List<float>();
            string[] strArray = str.Split(';');
            for (int i = 0; i < strArray.Length; i++) {
                retList.Add(float.Parse(strArray[i]));
            }
            return retList;
        }
        public static Color List2552Color(List<float> colorList) {
            return new Color(colorList[0] / 255, colorList[1] / 255, colorList[2] / 255, colorList[3] / 255);
        }

        public static Vector3 Deg2Rad(Vector3 euler) {
            Vector3 ret = new Vector3(180 < euler.x ? euler.x - 360 : euler.x, 180 < euler.y ? euler.y - 360 : euler.y, 180 < euler.z ? euler.z - 360 : euler.z);
            return ret * Mathf.Deg2Rad;
        }

        public static string Time2MMSS(int lastTime) {
            if (lastTime < 0) {
                return string.Empty;
            }
            return string.Format("{0:D2}:{1:D2}", lastTime / 60, lastTime % 60);
        }

        public static void ImageAutoSize(this Image image, Vector2 minSize, Vector2 maxSize) {
            image.SetNativeSize();
            RectTransform rectTransform = image.GetComponent<RectTransform>();
            if (rectTransform.sizeDelta.x < minSize.x) {
                rectTransform.sizeDelta *= minSize.x / rectTransform.sizeDelta.x;
            }
            if (rectTransform.sizeDelta.y < minSize.y) {
                rectTransform.sizeDelta *= minSize.y / rectTransform.sizeDelta.y;
            }
            if (maxSize.x < rectTransform.sizeDelta.x) {
                rectTransform.sizeDelta *= maxSize.x / rectTransform.sizeDelta.x;
            }
            if (maxSize.y < rectTransform.sizeDelta.y) {
                rectTransform.sizeDelta *= maxSize.y / rectTransform.sizeDelta.y;
            }
        }

        public static string MakeRelativePath(DirectoryInfo subPath, DirectoryInfo rootPath) {
            return MakeRelativePath(subPath.FullName, rootPath.FullName);
        }
        public static string MakeRelativePath(string subPath, string rootPath) {
            StringBuilder relativePath = new StringBuilder();
            string[] subArray = subPath.Replace('\\', '/').Split('/');
            string[] rootArray = rootPath.Replace('\\', '/').Split('/');
            for (int i = 0; i < subArray.Length; i++) {
                if (i < rootArray.Length) {
                    if (!subArray[i].Equals(rootArray[i])) {
                        return subPath;
                    }
                    continue;
                }
                relativePath.Append(subArray[i]);
                if (i + 1 < subArray.Length) {
                    relativePath.Append('/');
                }
            }
            return relativePath.ToString();
        }

        public static List<R> ToList<T, R>(this T[] sources, Func<T, R> changeFunc) {
            int[] vs;
            List<R> results = new List<R>();
            for (int i = 0; i < sources.Length; i++) {
                results.Add(changeFunc(sources[i]));
            }
            return results;
        }

        public static string ToString<T>(this T[] sources, string interval, Func<T, string> changeFunc) {

            List<T> list = new List<T>(sources);
            return list.ToString(interval, changeFunc);
        }

        public static string ToString<T>(this List<T> sources, string interval, Func<T, string> changeFunc) {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < sources.Count; i++) {
                result.Append(changeFunc(sources[i]));
                if (i < sources.Count - 1) {
                    result.Append(interval);
                }
            }
            return result.ToString();
        }

        public static string ReExtName(string filePath, string newExtName = null) {
            FileInfo fileInfo = new FileInfo(filePath);
            if (!string.IsNullOrEmpty(newExtName) && -1 == newExtName.IndexOf(".")) {
                newExtName = "." + newExtName;
            }
            return filePath.Substring(0, filePath.Length - fileInfo.Extension.Length) + newExtName;
        }

        public static Transform FindChild(this Transform trans, string goName) {
            Transform child = trans.Find(goName);
            if (child != null)
                return child;

            Transform go = null;
            for (int i = 0; i < trans.childCount; i++) {
                child = trans.GetChild(i);
                go = FindChild(child, goName);
                if (go != null)
                    return go;
            }
            return null;
        }

        public static T FindChild<T>(this Transform trans, string goName) where T : UnityEngine.Object {
            Transform child = trans.Find(goName);
            if (child != null) {
                return child.GetComponent<T>();
            }

            Transform go = null;
            for (int i = 0; i < trans.childCount; i++) {
                child = trans.GetChild(i);
                go = FindChild(child, goName);
                if (go != null) {
                    return go.GetComponent<T>();
                }
            }
            return null;
        }


        public static System.Text.Encoding GetEncoding(string filePath) {
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read)) {
                System.Text.Encoding encoding = GetEncoding(fileStream);
                return encoding;
            }
        }
        public static System.Text.Encoding GetEncoding(FileStream fileStream) {
            byte[] threeBytes;
            using (BinaryReader binaryReader = new BinaryReader(fileStream, System.Text.Encoding.Default)) {
                threeBytes = binaryReader.ReadBytes(3);
            }
            if (threeBytes[0] >= 0xEF) {
                if (threeBytes[0] == 0xEF && threeBytes[1] == 0xBB && threeBytes[2] == 0xBF) {
                    return System.Text.Encoding.UTF8;
                }
                else if (threeBytes[0] == 0xFE && threeBytes[1] == 0xFF) {
                    return System.Text.Encoding.BigEndianUnicode;
                }
                else if (threeBytes[0] == 0xFF && threeBytes[1] == 0xFE) {
                    return System.Text.Encoding.Unicode;
                }
                else {
                    return System.Text.Encoding.Default;
                }
            }
            else {
                return System.Text.Encoding.Default;
            }
        }

        public static void EncodingConvert(string filePath, Encoding target, Encoding srcFileCoding = null) {
            string content = string.Empty;
            if (null == srcFileCoding) {
                using (StreamReader sr = new StreamReader(filePath)) {
                    content = sr.ReadToEnd();
                }
            }
            else {
                using (StreamReader sr = new StreamReader(filePath, srcFileCoding)) {
                    content = sr.ReadToEnd();
                }
            }
            using (StreamWriter sw = new StreamWriter(filePath, false, target)) {
                sw.Write(content);
            }
        }

        public static string CSVTextHandle(string valueText) {
            if (null == valueText) {
                return string.Empty;
            }
            valueText = valueText.Replace("\"", "\"\"");
            if (valueText.Contains(",") || valueText.Contains("\"") || valueText.Contains("\r") || valueText.Contains("\n")) {
                valueText = string.Format("\"{0}\"", valueText);
            }
            return valueText;
        }

        public static void SaveTexture(string outPath, Texture2D texture2D) {
            byte[] bytes = texture2D.EncodeToPNG();
            File.WriteAllBytes(outPath, bytes);
        }

        public static void SaveTexture(string outPath, RenderTexture renderTexture) {
            int width = renderTexture.width;
            int height = renderTexture.height;
            Texture2D texture2D = new Texture2D(width, height, TextureFormat.ARGB32, false);
            RenderTexture oldActive = RenderTexture.active;
            RenderTexture.active = renderTexture;
            texture2D.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            texture2D.Apply();
            RenderTexture.active = oldActive;
            SaveTexture(outPath, texture2D);
        }

        public static Texture2D Sprite2Texture2D(Sprite sprite) {
            Texture2D texture = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
            // 复制有效颜色区块
            Color[] retColor = sprite.texture.GetPixels(
                (int)sprite.rect.x,
                (int)sprite.rect.y,
                (int)sprite.rect.width,
                (int)sprite.rect.height
            );
            texture.SetPixels(retColor);
            texture.Apply();
            return texture;
        }


        public static Sprite Texture2D2Sprite(Texture2D texture2D) {
            return Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), Vector2.zero, 1f);
        }

        public static void CallCallback(Action callback) {
            if (null != callback) {
                callback();
            }
        }

        public static void CallCallback<T>(Action<T> callback, T param) {
            if (null != callback) {
                callback(param);
            }
        }

        public static void CallCallback<T1, T2>(Action<T1, T2> callback, T1 param1, T2 param2) {
            if (null != callback) {
                callback(param1, param2);
            }
        }

        public static float GetClipLength(this Animator animator, string clipName) {
            AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
            for (int i = 0; i < clips.Length; i++) {
                if (clipName.Equals(clips[i].name)) {
                    return clips[i].length;
                }
            }
            return 0f;
        }

        public static T GetORAddComponent<T>(this GameObject gameObject) where T : Component {
            T component = gameObject.GetComponent<T>();
            if (null != component) {
                return component;
            }
            return gameObject.AddComponent<T>();
        }

        public static void SetAnchoredPosition(this GameObject gameObject, Vector2 position) {
            gameObject.GetComponent<RectTransform>().anchoredPosition = position;
        }

        public static void SetAnchoredPosition(this Transform transform, Vector2 position) {
            transform.GetComponent<RectTransform>().anchoredPosition = position;
        }

        public static Vector2 ScreenPoint2Local(this Transform transform, Camera camera, Vector2 screenPoint) {
            Vector2 worldPos = camera.ScreenToWorldPoint(screenPoint);
            return transform.InverseTransformPoint(worldPos);
        }

        public static Vector2 Local2ScreenPoint(this Transform transform, Camera camera, Vector2 localPos) {
            Vector2 worldPos = transform.TransformPoint(localPos);
            return camera.WorldToScreenPoint(worldPos);
        }

        public static void Update<T1, T2>(this Dictionary<T1, T2> dict, T1 key, T2 value) {
            if (null == dict) {
                return;
            }
            if (!dict.ContainsKey(key)) {
                dict.Add(key, value);
            } else {
                dict[key] = value;
            }
        }
    }
}