﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using UnityEngine;

namespace UNN
{

    [System.Serializable]
    public class Signal : IDisposable, ISerializationCallbackReceiver
    {

        public ComputeBuffer Buffer { get { return buffer; } }
        public int Rows { get { return rows; } }
        public int Columns { get { return columns; } }

        protected ComputeBuffer buffer;
        protected int rows, columns;

        public Signal(float[] v)
        {
            this.rows = 1;
            this.columns = v.Length;

            buffer = new ComputeBuffer(this.rows * this.columns, Marshal.SizeOf(typeof(float)));
            buffer.SetData(v);
        }

        public Signal(float[,] mat)
        {
            this.rows = mat.GetLength(0);
            this.columns = mat.GetLength(1);
            buffer = new ComputeBuffer(this.rows * this.columns, Marshal.SizeOf(typeof(float)));
            buffer.SetData(mat);
        }

        public Signal(int rows, int cols)
        {
            this.rows = rows;
            this.columns = cols;
            buffer = new ComputeBuffer(this.rows * this.columns, Marshal.SizeOf(typeof(float)));
        }

        public Signal(int rows, int cols, ComputeBuffer buf)
        {
            this.rows = rows;
            this.columns = cols;
            this.buffer = buf;
        }

        public Signal(Signal sig) : this(sig.rows, sig.Columns)
        {
        }

        public void Init(float v = 0f)
        {
            float[,] value = new float[rows, columns];
            for(int y = 0; y < rows; y++)
            {
                for(int x = 0; x < columns; x++)
                {
                    value[y, x] = v;
                }
            }
            buffer.SetData(value);
        }

        public float[,] GetData()
        {
            float[,] value = new float[rows, columns];
            buffer.GetData(value);
            return value;
        }

        public void Log()
        {
            float[,] value = GetData();

            var output = new string[rows + 1];
            output[0] = "[" + rows + ", " + columns + "]";

            for(int y = 0; y < rows; y++)
            {
                string[] row = new string[columns];
                for(int x = 0; x < columns; x++)
                {
                    row[x] = value[y, x].ToString("0.00000");
                }
                output[y + 1] = string.Join(",", row);
            }

            Debug.Log(string.Join("\n", output));
        }

        public void LogMNIST(float threshold = 0.5f)
        {
            float[,] value = GetData();

            var output = new string[28];
            for(int y = 0; y < 28; y++)
            {
                int offset = (28 - y - 1) * 28;
                string[] row = new string[28];
                for (int x = 0; x < 28; x++)
                {
                    row[x] = (threshold > value[0, offset + x]) ? "." : " ";
                }
                output[y] = string.Join("", row);
            }

            Debug.Log(string.Join("\n", output));
        }

        public void Dispose()
        {
            if(buffer != null)
            {
                buffer.Release();
            }
            buffer = null;
        }

        public void OnBeforeSerialize()
        {
        }

        public void OnAfterDeserialize()
        {
        }

    }

}


