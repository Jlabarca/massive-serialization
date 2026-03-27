using System;
using System.IO;

namespace Massive.Serialization
{
	/// <summary>
	/// Extension methods on Stream for reading/writing primitive types.
	/// Used by massive-netcode's client-server layer.
	/// Wraps the static helpers in SerializationUtils.
	/// </summary>
	public static class StreamExtensions
	{
		public static int ReadInt(this Stream stream)
		{
			return SerializationUtils.ReadInt(stream);
		}

		public static void WriteInt(this Stream stream, int value)
		{
			SerializationUtils.WriteInt(value, stream);
		}

		public static byte Read1Byte(this Stream stream)
		{
			return SerializationUtils.ReadByte(stream);
		}

		public static void Write1Byte(this Stream stream, byte value)
		{
			SerializationUtils.WriteByte(value, stream);
		}

		public static double ReadDouble(this Stream stream)
		{
			Span<byte> buffer = stackalloc byte[sizeof(double)];
			stream.Read(buffer);
			return BitConverter.ToDouble(buffer);
		}

		public static void WriteDouble(this Stream stream, double value)
		{
			Span<byte> buffer = stackalloc byte[sizeof(double)];
			BitConverter.TryWriteBytes(buffer, value);
			stream.Write(buffer);
		}

		public static short ReadShort(this Stream stream)
		{
			Span<byte> buffer = stackalloc byte[sizeof(short)];
			stream.Read(buffer);
			return BitConverter.ToInt16(buffer);
		}

		public static void WriteShort(this Stream stream, short value)
		{
			Span<byte> buffer = stackalloc byte[sizeof(short)];
			BitConverter.TryWriteBytes(buffer, value);
			stream.Write(buffer);
		}

		public static void ReadAllocator(this Stream stream, Allocator allocator)
		{
			SerializationUtils.ReadAllocator(allocator, stream);
		}

		public static void WriteAllocator(this Stream stream, Allocator allocator)
		{
			SerializationUtils.WriteAllocator(allocator, stream);
		}

		/// <summary>
		/// Polyfill for Stream.ReadExactly (.NET 7+).
		/// Reads exactly buffer.Length bytes, throwing if the stream ends early.
		/// </summary>
		public static void ReadExactly(this Stream stream, Span<byte> buffer)
		{
			int totalRead = 0;
			while (totalRead < buffer.Length)
			{
				int read = stream.Read(buffer.Slice(totalRead));
				if (read == 0)
					throw new EndOfStreamException("Unexpected end of stream.");
				totalRead += read;
			}
		}
	}
}
