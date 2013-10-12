using System;

namespace Publisher.Utilities
{
	// 
	// Array manipulation 
	//

	public sealed class ArrayManipulation
	{
		public static Array Resize(Array array, int newSize)
		{
			Array newArray = Array.CreateInstance(array.GetType().GetElementType(), newSize);
			Array.Copy(array, 0, newArray, 0, array.Length);

			return newArray;
		}

		public static Array ResizeAtBeginning(Array array, int newSize)
		{
			Array newArray = Array.CreateInstance(array.GetType().GetElementType(), newSize);
			Array.Copy(array, 0, newArray, 1, array.Length);

			return newArray;
		}

		public static Array Insert(Array array, object element)
		{
			int len = array.Length;

			Array newArray = Resize(array, array.Length + 1);
			newArray.SetValue(element, len);

			return newArray;
		}

		public static Array InsertAtBeginning(Array array, object element)
		{
			Array newArray = ResizeAtBeginning(array, array.Length + 1);
			newArray.SetValue(element, 0);

			return newArray;
		}
		
		public static Array Remove(Array array, int index)
		{
			int len = array.Length;
			Array newArray = Array.CreateInstance(array.GetType().GetElementType(), len - 1);

			Array.Copy(array, 0, newArray, 0, index);
			Array.Copy(array, index + 1, newArray, index, len - index - 1);

			return newArray;
		}
	}
}