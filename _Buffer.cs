using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Text;
#if TRUE_SYNC
using TrueSync;
#endif
using Unity.Profiling.LowLevel.Unsafe;
using UnityEditor;


public enum AppendType
{
    None,
    Int,
    //Byte,
    Bool,
    String,
    Float,
    Short,
    Long,
    ArrayLength,
    FP,
}
public delegate int BufferEvent(AppendType appendType, int index); //1 to change the value
public partial class Buffer2
{
    public static Buffer2 globalBuffer = new Buffer2();
    public const int GLOBALNULL = int.MinValue + 11111;

    public event BufferEvent onAppend;

    public bool isRuntime { get; set; } = true;
    public bool isEditime { get; set; } = true;
    public bool isLoad => mode == BufferMode.Load;
    public bool isSave => mode == BufferMode.Save;
    public static bool boolValue
    {
        get { return byteValue != 0; }
        set { byteValue = (byte)(value ? 1 : 0); }
    }
    public static int intValue
    { get { return (int)longValue; } set { longValue = value; } }
    public static byte byteValue
    {
        get { return (byte)intValue; }
        set { intValue = value; floatValue = value; /*fpValue = value;*/ }
    }
    public static short shortValue
    {
        get { return (short)intValue; }
        set { intValue = value; }
    }
#if TRUE_SYNC
   public static FP fpValue
   {
       get { return _fpValue; }
       set {
           floatValue = value.AsFloat();
           _fpValue = value;
       }
   }
#endif
    public static float floatValue { get; set; }
    public static string strValue { get; set; }
    public static long longValue { get; set; }
    // public static FP fpValue { g}

#if TRUE_SYNC
    static FP _fpValue;
#endif
    //   public JSONObject jsonObject { get; set; }
    public byte[] ToFileBytes()
    {
        List<byte> types = new List<byte>(this.data.Count / 4);
        List<byte> values = new List<byte>(this.data.Count * 2);
        sbyte currentType = 0;
        const sbyte BOOL_BYTE = 1;
        const sbyte INT = 0;
        const sbyte FLOAT = 2;
        const sbyte STRING = 3;


        sbyte currentTypeCounter = 0;
        for (int i = 0; i < this.data.Count; i++)
        {
            var x = this.data[i];
            TypeFlag f = TypeFlag.Int;
            #region TYPE CHECK
            if (x is int)
            {
                currentType |= (sbyte)(INT << (currentTypeCounter * 2));
            }
            else if (x is bool)
            {
                currentType |= (sbyte)(BOOL_BYTE << (currentTypeCounter * 2));
                f = TypeFlag.BoolByte;

                var bv = (bool)x;
                if (bv == false)
                    x = (byte)0;
                else
                    x = (byte)1;


            }
            else if (x is byte)
            {
                currentType |= (sbyte)(BOOL_BYTE << (currentTypeCounter * 2));
                f = TypeFlag.BoolByte;

            }
            else if (x is float)
            {
                currentType |= (sbyte)(FLOAT << (currentTypeCounter * 2));
                f = TypeFlag.Float;

            }
            else if (x is string)
            {
                currentType |= (sbyte)(STRING << (currentTypeCounter * 2));
                f = TypeFlag.String;

            }

            currentTypeCounter++;

            if (currentTypeCounter == 4 || i >= this.data.Count - 1)
            {
                currentTypeCounter = 0;
                types.Add((byte)currentType);
            }
            #endregion

            byte[] bArray;
            switch (f)
            {
                case TypeFlag.Int:
                    bArray = BitConverter.GetBytes((int)x);
                    break;
                case TypeFlag.BoolByte:
                    bArray = new byte[] { (byte)x };
                    break;
                case TypeFlag.Float:
                    bArray = BitConverter.GetBytes((float)x);
                    break;
                case TypeFlag.String:
                    var utf8Array = Encoding.UTF8.GetBytes((string)x);
                    var sizeArray = BitConverter.GetBytes(utf8Array.Length);
                    bArray = new byte[utf8Array.Length + sizeArray.Length];
                    sizeArray.CopyTo(bArray, 0);
                    utf8Array.CopyTo(bArray, sizeArray.Length);
                    break;
                default:
                    bArray = new byte[0];
                    break;
            }

            for (int j = 0; j < bArray.Length; j++)
            {
                values.Add(bArray[j]);
            }

        }

        //4 + 4 -> (amount of data rows) + (total byte length)

        int totalByteLength = types.Count + values.Count + 4 + 4;
        byte[] delta = new byte[totalByteLength];
        var totalByteLengthArray = BitConverter.GetBytes(totalByteLength);
        var rowsCountArray = System.BitConverter.GetBytes(types.Count);

        totalByteLengthArray.CopyTo(delta, 0);
        rowsCountArray.CopyTo(delta, 4);
        types.CopyTo(delta, 8);
        values.CopyTo(delta, 8 + types.Count);

        return delta;
    }
    //int RoundDiv(int a, int b) {
    //    var v = a / b;
    //    var m = a % b;
    //    if (m != 0)
    //        v++;
    //    return v;
    //}
    public void LoadFromBytes(byte[] array)
    {
        //     Buffer2 b = new Buffer2 ();
        int totalLength = BitConverter.ToInt32(array, 0);
        int rowCount = BitConverter.ToInt32(array, 4);
        data.Capacity = rowCount;
        int pointer = 8 + rowCount;

        for (int i = 0; i < rowCount; i++)
        {
            var x = i + 8;
            var y = array[x];
            var ys = (sbyte)array[x];
            for (int j = 0; j < 4; j++)
            {
                if (pointer >= totalLength)
                    return;
                TypeFlag f = TypeFlag.Int;
                //var mj = j + x;
                //var typeFlag 

                ys = (sbyte)(ys >> 2);
                sbyte com = (sbyte)((ys & 1) | (ys & 2));

                f = (TypeFlag)(1 << com);

                try
                {

                    switch (f)
                    {
                        case TypeFlag.Int:
                            data.Add(BitConverter.ToInt32(array, pointer));
                            pointer += 4;
                            break;
                        case TypeFlag.BoolByte:
                            data.Add(array[pointer]);
                            pointer++;
                            break;
                        case TypeFlag.Float:
                            data.Add(BitConverter.ToSingle(array, pointer));
                            pointer += 4;
                            break;
                        case TypeFlag.String:
                            var strLength = BitConverter.ToInt32(array, pointer);
                            var utf = Encoding.UTF8;

                            var str = utf.GetString(array, pointer + 4, strLength);
                            data.Add(str);
                            pointer += strLength + 4;
                            break;
                    }
                }
                catch
                {
                    Debug.LogError(
                    string.Format("error happen with pointer {0} the total data had been entere is {1}"
                    , pointer, data.Count)
                    );
                }

            }

        }
        //   return b;
    }
    enum TypeFlag : sbyte
    {
        None,
        Int = 1,
        BoolByte = 2,
        Float = 4,
        String = 8,
    }



    //  b.AppendWholeSerialize ( tar );

    // static List<int> buffersCount = new List<int> ();
    public BufferMode mode
    {
        get { return _mode; }
        set
        {
            _mode = value;
            index = 0;
        }
    }
    //public int specialClass
    //{
    //    get { return _specialClass - 1; }
    //    set {
    //        var old = specialClass;
    //        _specialClass = value +1;
    //        //if(old != specialClass)
    //        if(specialClass >= 0 && specialClass < buffersCount.Count ) {
    //            if(buffersCount[specialClass] > 0)
    //            this.data = new List<object>
    //                ( buffersCount[specialClass] );
    //            else {
    //                registerSpecialClass = true;
    //            }
    //        }else{

    //        }
    //        if(specialClass >= 0) {
    //            if (buffersCount.Count <= specialClass)
    //                buffersCount.Insert ( specialClass, 0 );
    //        }

    //    }
    //}
    BufferMode _mode;

    /// <summary>
    /// all supported data types is:
    /// int, bool, string, float
    /// </summary>
    readonly List<object> data = new List<object>();

    public int dataSize { get { return data.Count; } }

    int _index = 0;
    int meanIndex;

    bool dontInvokeAppend;
    int index
    {
        get { return _index; }
        set { _index = value; if (data.Count >= data.Capacity) data.Capacity = GetPreferCapacity(data.Count); }
    }
    //int _specialClass;
    // bool registerSpecialClass;

    static ISerialize __temp;
    /// <summary>
    /// classes only
    /// for structs use clone
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>



    int emptyBufferMethod(AppendType at, int x) { return 0; }

    public Buffer2()
    {
        InitSize(100);
        onAppend = emptyBufferMethod;
    }

    int Invoke(AppendType at, int x)
    {

        if (dontInvokeAppend)
        {
            dontInvokeAppend = false;
            return 0;
        }
        return onAppend(at, x);
    }


    //public void FieldName(string str) { }
    //public void EndContext() { }

    static int GetPreferCapacity(int count)
    {

        int x = count + Mathf.RoundToInt(count * 0.15f);
        return Mathf.Max(x, 100);
    }
    static void long2doubleInt(long a, out int a1, out int a2)
    {
        a1 = (int)(a & uint.MaxValue);
        a2 = (int)(a >> 32);
        //return new int[] { a1, a2 };
    }
    static long doubleInt2long(int a1, int a2)
    {
        long b = a2;
        b = b << 32;
        b = b | (uint)a1;
        return b;
    }

    public static void CopyTo(ISerialize a, ISerialize b)
    {
        Buffer2 x = new Buffer2();
        x.mode = BufferMode.Save;
        a.WholeSerialize(x);
        x.index = 0;
        x.mode = BufferMode.Load;
        b.WholeSerialize(x);
        __temp = b;
    }
    public static T Clone<T>(T source) where T : ISerialize, new()
    {
        T clone = new T();
        CopyTo(source, clone);

        clone = (T)__temp;

        return clone;
    }
    public static Buffer2 FromBytes(byte[] bytes)
    {
        Buffer2 b = new Buffer2();
        b.LoadFromBytes(bytes);
        return b;
    }
    //public static void CopyStructs<T>(T source,ref T target) where T:struct,ISerialize {
    //    T x = new T ();
    //    CopyTo ( source, x );
    //    target = x;

    //}

    /// <summary>
    /// used like this:
    /// this.array = BeginArray(this.array)
    /// just will return a new instance in Load Mode
    /// </summary>
    //public void BeginArray<T>(ref TArray<T> array) {

    //    int count = array == null ? 0 : array.Count;
    //    Append ( ref count );
    //    if (mode == BufferMode.Save) {
    //        //  return array;
    //    }
    //    else if (mode == BufferMode.Load) {
    //        array = new TArray<T> ( new T[count] );
    //    }
    //    //return null;
    //}
    public void BeginArray<T>(ref T[] array)
    {
        int count = array == null ? 0 : array.Length;
        dontInvokeAppend = true;
        Append(ref count);
        intValue = count;
        if (this.Invoke(AppendType.ArrayLength, ++meanIndex) != 0)
            count = intValue;
        if (mode == BufferMode.Save)
        {
            //  return array;
        }
        else if (mode == BufferMode.Load)
        {
            array = new T[count];
        }

    }
    //public void AppendArray<T>(ref TArray<T> array) {
    //    int count = array == null ? 0 : array.Count;
    //    Append ( ref count );
    //    if (mode == BufferMode.Save) {
    //        //  return array;
    //    }
    //    else if (mode == BufferMode.Load) {
    //        array = new TArray<T> ( new T[count] );
    //    }
    //    for (int i = 0; i < array.Count; i++) {
    //        Append
    //   }
    //}
    //public void EndArray() { }

    public void Clear()
    {
        data.Clear();
        index = 0;
        _mode = 0;
    }
    //[System.Obsolete ( "dont realy make reliable method" )]
    //public void InitSize(ElementType et) {
    //    InitSize ( DataMod.GetElementSize ( et ) );
    //}
    public void InitSize(int size)
    {
        this.data.Capacity = size;
    }
    public void Append(ref byte b)
    {
        if (mode == BufferMode.Save)
            this.data.Add(b);
        else
        if (mode == BufferMode.Load)
            b = (byte)this.data[index];

        byteValue = b;
        if (this.Invoke(AppendType.Int, ++meanIndex) != 0)
            b = byteValue;


        //meanIndex++;
        index++;
    }
    public void Append(ref bool b)
    {

        dontInvokeAppend = true;
        var bx = b ? 1 : 0;
        Append(ref bx);
        b = bx == 0 ? false : true;
        boolValue = b;
        if (this.Invoke(AppendType.Bool, ++meanIndex) != 0)
            b = boolValue;
        //meanIndex++;
    }
    public void Append(ref int i)
    {
        if (mode == BufferMode.Save)
            data.Add(i);
        if (mode == BufferMode.Load)
            i = (int)this.data[index];
        intValue = i;
        if (this.Invoke(AppendType.Int, ++meanIndex) != 0)
            i = intValue;

        index++;
    }
    public void Append(ref short i)
    {
        if (mode == BufferMode.Save)
            data.Add((int)i);
        if (mode == BufferMode.Load)
            i = (short)(int)this.data[index];

        shortValue = i;
        if (this.Invoke(AppendType.Short, ++meanIndex) != 0)
            i = shortValue;
        index++;
    }

    public void Append(ref long L)
    {
        int a = 0, b = 0;
        if (mode == BufferMode.Save)
        {
            long2doubleInt(L, out a, out b);
        }
        Append(ref a);
        Append(ref b);
        if (mode == BufferMode.Load)
        {
            L = doubleInt2long(a, b);
        }
        longValue = L;
        if (this.Invoke(AppendType.Long, ++meanIndex) != 0)
            L = longValue;
    }
#if TRUE_SYNC
   public void Append(ref FP f) {
       long x = f._serializedValue;
       Append ( ref x );

       //if (mode == BufferMode.Save)
       //  data.Add ( f._serializedValue );
       if (mode == BufferMode.Load)
           f = FP.FromRaw ( x );
       fpValue = f;
       if (this.Invoke ( AppendType.FP, ++meanIndex ) != 0)
           f = fpValue;
       //index++;
   }
#endif

    public void Append(ref string s)
    {
        if (mode == BufferMode.Save)
            data.Add(s);
        if (mode == BufferMode.Load)
            s = (string)this.data[index];
        strValue = s;
        if (this.Invoke(AppendType.String, ++meanIndex) != 0)
            s = strValue;
        index++;
    }
    public void Append(ref float f)
    {
        if (mode == BufferMode.Save)
        {
            data.Add(f);
        }
        else if (mode == BufferMode.Load)
        {
            f = (float)data[index];
        }
        floatValue = f;
        if (this.Invoke(AppendType.Float, ++meanIndex) != 0)
            f = floatValue;
        index++;
    }
    public void Append(ref Vector3 v)
    {
        float x = v.x, y = v.y, z = v.z;
        Append(ref x);
        Append(ref y);
        Append(ref z);
        v = new Vector3(x, y, z);
        //if (mode == BufferMode.Save) {
        //    data.Add ( v.x );
        //    data.Add ( v.y );
        //    data.Add ( v.z );
        //}
        //else if (mode == BufferMode.Load) {
        //    v = new Vector3 {
        //        x = (float)data[index],
        //        y = (float)data[index + 1],
        //        z = (float)data[index + 2]
        //    };
        //}
        //index += 3;
    }
    public void Append(ref Vector2 v)
    {
        float x = v.x, y = v.y;
        Append(ref x);
        Append(ref y);
        v = new Vector3(x, y);

        //if (mode == BufferMode.Save) {
        //    data.Add ( v2.x );
        //    data.Add ( v2.y );
        //}
        //else if (mode == BufferMode.Load) {
        //    v2 = new Vector2 (
        //        (float)data[index],
        //        (float)data[index + 1]
        //        );
        //}
        //index += 2;
    }

    public void Append(ref Vector2Int v)
    {
        int x = v.x;
        int y = v.y;
        Append(ref x);
        Append(ref y);
        v = new Vector2Int(v.x, v.y);
    }
    public void AppendField<T>(ref T s) where T : class, ISerialize, new()
    {

        //if(s == null) {
        //    throw new Exception ( "FATAL ERROR, field cannot be null" );
        //}
        if (mode == BufferMode.Save)
        {
            if (s == null)
            {
                int xy = GLOBALNULL;
                Append(ref xy);
            }
            else
                s.FieldSerialize(this);

            //return saveModeObj;
        }
        if (mode == BufferMode.Load)
        {
            var currentValue = data[index];
            if (currentValue is int)
            {
                var currentValueInt = (int)currentValue;
                if (currentValueInt == GLOBALNULL)
                {
                    s = null;
                    index++;
                    return;
                }
            }
            T x = new T();
            x.FieldSerialize(this);
            //loadModeObj.FieldSerialize ( this );
            s = x;
        }
    }
    public void AppendWholeSerialize<T>(ref T s) where T : class, ISerialize
    {
        if (mode == BufferMode.Save)
        {
            if (s == null)
            {
                int xy = GLOBALNULL;
                Append(ref xy);
            }
            else
                s.WholeSerialize(this);

            //return saveModeObj;
        }
        if (mode == BufferMode.Load)
        {
            var currentValue = data[index];
            if (currentValue is int)
            {
                var currentValueInt = (int)currentValue;
                if (currentValueInt == GLOBALNULL)
                {
                    s = null;
                    index++;
                    return;
                }
            }
            //T x = new T ();
            s.WholeSerialize(this);
            //loadModeObj.FieldSerialize ( this );
            //s = x;
        }
    }

    public void Append(ref DateTime d)
    {
        var l = d.ToBinary();
        Append(ref l);
        d = DateTime.FromBinary(l);
    }
    //public void Append(ref TransformData td) {
    //    var x = td;
    //    Vector3 pos = td.position,
    //        rot = td.rotation,
    //        sca = td.scale;
    //    Append ( ref pos );
    //    Append ( ref rot );
    //    Append ( ref sca );
    //    //  if (mode == BufferMode.Load)
    //    td.position = pos;
    //    td.rotation = rot;
    //    td.scale = sca;
    //    //     td = x;
    //}
    //public void Append(ref StoredResource s) {
    //    var m = s;
    //    int mode = (int)s.mode;
    //    int res = (int)s.resource;
    //    var amount = s.amount;
    //    Append ( ref mode );
    //    Append ( ref res );
    //    Append ( ref amount );

    //    m.mode = (StoredResourceMode)mode;
    //    m.resource = (ResourceID)res;
    //    m.amount = amount;

    //    s = m;
    //}
    //public void Append(ref GraphicType graphicType) {
    //    int x = (int)graphicType;
    //    Append ( ref x );
    //    graphicType = (GraphicType)x;
    //}
    //public void Append(ref EffectType et) {
    //    int x = (int)et;
    //    Append ( ref x );
    //    et = (EffectType)x;
    //}
    //public void Append(ref GraphicDelta gd) {
    //    int targetGraphicID = gd.graphicID;
    //    var td = gd.additiveTransform;

    //    Append ( ref targetGraphicID );
    //    Append ( ref td );


    //    if (mode == BufferMode.Load)
    //        gd = new GraphicDelta ( targetGraphicID, td );

    //}
    //public void Append(ref EffectCommand ec) {
    //    EffectType et = ec.type;
    //    int id1 = ec.id1;
    //    int id2 = ec.id2;
    //    FP v1 = ec.value1;
    //    FP v2 = ec.value2;


    //    Append ( ref et );
    //    Append ( ref id1 );
    //    Append ( ref id2 );
    //    Append ( ref v1 );
    //    Append ( ref v2 );

    //    ec = new EffectCommand ( et, id1, id2, v1, v2 );
    //}
    //public void Append(ref TechRequire tr) {
    //    int rid = (int)tr.resourceID;
    //    FP f = tr.resourceValue;

    //    Append ( ref rid );
    //    Append ( ref f );


    //    tr = new TechRequire ( (ResourceID)rid, f );
    //}
    //public void Append(ref LogicMode lm) {
    //    int x = (int)lm;
    //    Append ( ref x );
    //    lm = (LogicMode)x;
    //}

}

public enum BufferMode
{
    Save,//save game
    Load,//load game
}
/// <summary>
/// type must not be struct
/// </summary>
public interface ISerialize///convert current to files
{

    //how to serialize this class if it is a field on another class
    void FieldSerialize(Buffer2 b);

    void WholeSerialize(Buffer2 b);//whole serialization to class
}

public delegate void BufferFormat(Buffer2 b);