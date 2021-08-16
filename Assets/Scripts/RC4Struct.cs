using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RC4Struct : SingleObject<RC4Struct>
{
    int paramX = 0;
    int paramY = 0;
    byte[] mBaseByte = new byte[256];
    /// <summary>
    /// 初始化
    /// </summary>
    public void Init() {
        int index = 0;
        paramX = paramY = 0;
        for (byte i = 0; i <= 255; i++) {
            mBaseByte[i] = i;
        }
        for (byte i = 0; i <= 255; i++) {
            index = (index + mBaseByte[i]) & 255;
            mBaseByte[i] ^= mBaseByte[index];
            mBaseByte[index] ^= mBaseByte[i];
            mBaseByte[i] ^= mBaseByte[index];
        }
    }

    /// <summary>
    /// 生成密码流的下一个字节
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    public byte NextByte() {
        paramX = (paramX + 1) & 255;
        paramY = (paramY + mBaseByte[paramX]) & 255;
        mBaseByte[paramX] ^= mBaseByte[paramY];
        mBaseByte[paramY] ^= mBaseByte[paramX];
        mBaseByte[paramX] ^= mBaseByte[paramY];
        return mBaseByte[(mBaseByte[paramX] + mBaseByte[paramY]) & 255];
    }

    /// <summary>
    /// 生成密码流的下四个字节
    /// </summary>
    /// <returns></returns>
    public ulong NextuLong() {
        return (ulong)NextByte() + ((ulong)NextByte() << 8) + ((ulong)NextByte() << 16) + ((ulong)NextByte() << 24);
    }
}
