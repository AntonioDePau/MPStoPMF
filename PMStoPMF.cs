using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace mpsToPmf{
	public static class StringExtensions{
		public static List<byte> AddDecimalAsByte(this List<byte> a, int i, int l=2, bool r=false){
			byte[] b = BitConverter.GetBytes(i);
			if(!r){
				for(int x=0;x<l;x++){
					a.Add(b[x]);
				}
			}else{
				for(int x=l-1;x>-1;x--){
					a.Add(b[x]);
				}
			}
			return a;
		}
	}
	
	static class mpsToPmf{
		static string ReadHexByteAt(this MemoryStream p, int startPos, int length, bool be=false, bool ads=false, string stopAt=null){
			string lh = "";
			int hexIn;
			int c = 0;
			p.Seek(startPos,SeekOrigin.Begin);
			while((hexIn = p.ReadByte()) != -1 && c<length){
				c++;
				string h = string.Format("{0:X2}", hexIn);
				if(stopAt!=null){
					c=0;
					if(h==stopAt) break;
				}
				if(!be) lh+=h+(ads?" ":"");
				if(be) lh=h+(ads?" ":"")+lh;
			}
			return lh;
		}
		
		static List<byte> header = new List<byte>(){80,83,77,70,48,48};
		static int type = 15;
		static List<byte> headerBis = new List<byte>(){0, 0, 0, 78, 0, 0, 0, 1, 95, 144, 0, 0, 0, 105, 111, 117, 0, 0, 97, 168, 0, 1, 95, 144, 2, 1, 0, 0, 0, 52, 0, 0, 0, 1, 95, 144, 0, 0, 0, 105, 111, 117, 0, 1, 0, 0, 0, 34, 0, 2, 224, 0, 33, 239, 0, 0, 0, 0, 0, 0, 0, 0, 30, 17, 0, 0, 189, 0, 32, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
		
		static void addPMF(string f){
			MemoryStream ms = new MemoryStream();
			using(FileStream fs = File.OpenRead(f)){
				fs.CopyTo(ms);
			}
			if(ms.ReadHexByteAt(0,4)=="50534D46") Console.WriteLine("Is already PMF!");
			Console.WriteLine("Converting: "+f);
			Console.WriteLine("Size: "+ms.Length);
			List<byte> h = new List<byte>(header);
			string typeString = type.ToString();
			typeString = type<10?"0"+typeString:typeString;
			for(int i=0;i<typeString.Length;i++){
				h.Add((byte)(int)typeString[i]);
			}
			h = h.AddDecimalAsByte(2048,4, true);
			h = h.AddDecimalAsByte((int)ms.Length,4, true);
			h.AddRange(Enumerable.Repeat((byte)0, 64).ToList());
			h.AddRange(headerBis);
			h.AddRange(Enumerable.Repeat((byte)0, 2048-h.Count).ToList());
			h.AddRange(ms.ToArray().ToList());
			File.WriteAllBytes(Path.GetFileNameWithoutExtension(f)+".PMF", h.ToArray());
			Console.WriteLine("Done!");
		}
		
		static void Main(string[] args){
			if(args.Length>0){
				for(int i=0;i<args.Length;i++){
					if(args[i].IndexOf("type")==0){
						type = Int32.Parse(args[i].Split(':')[1]);
						continue;
					}
					if(File.Exists(args[i])) addPMF(args[i]);
				}
			}else{
				Console.WriteLine("Please specify at least one MPS file!");
			}
			Console.ReadLine();
		}
	}	
}
