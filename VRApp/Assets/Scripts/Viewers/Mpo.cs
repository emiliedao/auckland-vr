using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Viewers
{
	/// <summary>
	/// This class is used to handle Mpo files
	/// In particular, to retrieve left and right images separately from the Mpo file
	/// </summary>
	public static class Mpo
	{
		/// <summary>
		/// This function is used to separate an Mpo image into 2 textures (left and right) 
		/// </summary>
		/// <param name="fileName"></param>
		/// <returns></returns>
		public static List<Texture2D> GetMpoImages(string fileName)
		{
			var textures = new List<Texture2D>();
			byte[] tempBytes = new byte[100];

			using (var f = new FileStream(fileName, FileMode.Open, FileAccess.Read))
			{
				tempBytes = new byte[f.Length];
				f.Read(tempBytes, 0, (int)f.Length);
			}

			List<int> imageOffsets = new List<int>();
			int offset = 0, tempOffset = 0;
			byte[] keyBytes = { 0xFF, 0xD8, 0xFF, 0xE1 };
			byte[] keyBytes2 = { 0xFF, 0xD8, 0xFF, 0xE0 };

			while (true)
			{
				tempOffset = SearchBytes(tempBytes, keyBytes, offset, tempBytes.Length);
				if (tempOffset == -1)
					tempOffset = SearchBytes(tempBytes, keyBytes2, offset, tempBytes.Length);
				if (tempOffset == -1) break;
				offset = tempOffset;
				imageOffsets.Add(offset);
				offset += 4;
			}

			for (int i = 0; i < imageOffsets.Count; i++)
			{
				int length;
				if (i < (imageOffsets.Count - 1))
					length = imageOffsets[i + 1] - imageOffsets[i];
				else
					length = tempBytes.Length - imageOffsets[i];

				MemoryStream stream = new MemoryStream(tempBytes, imageOffsets[i], length);
				Texture2D texture = new Texture2D(4, 4);
				texture.LoadImage(stream.ToArray());
				textures.Add(texture);
			}

			return textures;
		}
	
		/// <summary>
		/// Searches an array of bytes for a byte pattern specified in another array.
		/// </summary>
		/// <param name="bytesToSearch"> Array of bytes to search</param>
		/// <param name="matchBytes">Byte pattern to search for</param>
		/// <param name="startIndex">Starting index within the first array to start searching</param>
		/// <param name="count">Number of bytes in the first array to search</param>
		/// <returns>Zero-based index of the beginning of the byte pattern found in 
		/// the byte array, or -1 if not found.</returns>
		public static int SearchBytes(byte[] bytesToSearch, byte[] matchBytes, int startIndex, int count)
		{
			int ret = -1, max = count - matchBytes.Length + 1;
			bool found;
			for (int i = startIndex; i < max; i++)
			{
				found = true;
				for (int j = 0; j < matchBytes.Length; j++)
				{
					if (bytesToSearch[i + j] != matchBytes[j]) { found = false; break; }
				}
				if (found) { ret = i; break; }
			}
			return ret;
		}

	}
}