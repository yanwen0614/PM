using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PM.utils;
using JiebaNet.Segmenter.PosSeg;
using PM.utils.TFIDF;

namespace PM.utils.TFIDF
{
    class GetP : CalcuDegree
    {
        
        public Dictionary<String, Double> DictionaryINmax = new Dictionary<String, Double>();

        private static List<String> FileList = new List<String>(); // the list of file 获取文件名

        public static String GetFileNameWithSuffix(String pathandname)
        {
            int start = pathandname.LastIndexOf("\\");
            if (start != -1)
                return pathandname.Substring(start + 1);
            return null;
        }

        // get list of file for the directory, including sub-directory of it
        public static List<String> ReAddirs(String filepath) {
            File file = new File(filepath);
			if (!file.isDirectory()) {
				System.out.println("输入的[]");
                System.out.println("filepath:" + file.getAbsolutePath());
            }
            else {
				    String[] flist = file;
				    for (int i = 0; i<flist.Length; i++) {
					    File newfile = new File(filepath + "\\" + flist[i]);
					    if (!newfile.isDirectory()) {
						    FileList.Add(newfile.getAbsolutePath());
					    } else if (newfile.isDirectory()) // if file is a directory, call ReAddirs
						    ReAddirs(filepath + "\\" + flist[i]);
				    }
			}
		    return FileList;
	    }

	// read file
	    public static String ReadFile(String file){
		    StringBuffer strSb = new StringBuffer(); // String is constant，StringBuffer can be  changed.
            InputStreamReader inStrR = new InputStreamReader(new FileInputStream(file), "gbk"); // byte streams to character streams
            BufferedReader br = new BufferedReader(inStrR);
            String line = br.readLine();
		    while (line != null) {
			    strSb.append(line).append("\r\n");
                line = br.readLine();
		    }

		    return strSb.toString();
	    }

        public GetP(int casetype) :base(casetype)  {
            
        }

        // term frequency in a file, times for each word
        protected  Dictionary<String, int> NormalTF(List<Pair> cutWordResult)
        {
            return base.NormalTF(cutWordResult);
        }

        // term frequency in a file, frequency of each word
        protected Dictionary<String, float> Tf(List<Pair> cutWordResult)
        {
            return base.Tf(cutWordResult);
        }

        // tf times for file
        protected Dictionary<String, Dictionary<String, int>> NormalTFAllFiles(String dirc)
        {
            Dictionary<String, Dictionary<String, int>> allNormalTF = new Dictionary<String, Dictionary<String, int>>();
		    List<String> filelist = ReAddirs(dirc);
            allNormalTF = base.NormalTFOfAll(filelist);
		    return allNormalTF;
	    }

        // tf for all file
        protected Dictionary<String, Dictionary<String, float>> TfAllFiles(String dirc)
        {
            Dictionary<String, Dictionary<String, float>> allTF = new Dictionary<String, Dictionary<String, float>>();
		    List<String> filelist = ReAddirs(dirc);
            allTF = base.TfOfAll(filelist);
		    return allTF;
	    }

        protected Dictionary<String, float> Idf(Dictionary<String, Dictionary<String, int>> all_tf)
        {
            return base.Idf(all_tf);
        }

	    public static void Tf_idf(Dictionary<String, Dictionary<String, int>> all_tf, Dictionary<String, float> idfs,
            String putpath) 
        {
            Dictionary<String, Dictionary<String, float>> resTfIdf = new Dictionary<String, Dictionary<String, float>>();
            int docNum = FileList.size();
            for (int i = 0; i<docNum; i++) {
                String filepath = FileList.get(i);
                Dictionary<String, Float> tfidf = new Dictionary<String, Float>();
                Dictionary<String, Float> temp = all_tf.get(filepath);
                Iterator iter = temp.entrySet().iterator();
                while (iter.hasNext()) {
                    Dictionary.Entry entry = (Dictionary.Entry)iter.next();
                    String word = entry.getKey().toString();
                    Float value = (float)Float.parseFloat(entry.getValue().toString()) * idfs.get(word);
                    tfidf.put(word, value);
                    }
                resTfIdf.put(filepath, tfidf);
                }
            DisTfIdf(resTfIdf, putpath);
        }

	// 排序算法
        public static void Rank(Dictionary<String, float> wordDictionary, String filename)
        {
            BufferedWriter Writer = new BufferedWriter(

                        new OutputStreamWriter(new FileOutputStream(new File(filename)), "utf-8"));
	            List<String> wordgaopindipin = new List<String>();
            List<Dictionary.Entry<String, Float>> list = new List<Dictionary.Entry<String, Float>>(wordDictionary.entrySet());
            Collections.sort(list, new Comparator<Dictionary.Entry<String, Float>>() {
		            // 降序排序
		            public int compare(Entry<String, Float> o1, Entry<String, Float> o2)
            {
            // return o1.getValue().compareTo(o2.getValue());
            return o2.getValue().compareTo(o1.getValue());
            }
	            });
	            // 排序靠前的60个词及权值
	            if (list.size() > 60) {
		            for (int i = 0; i< 59; i++) {
			            // 写入文件
			            wordgaopindipin.Add(list.get(i).getKey());
			            Writer.append(list.get(i).getKey() + " " + list.get(i).getValue() + "\r\n");
		            }
	            } else {
		            for (int i = 0; i<list.size(); i++) {
			            // 写入文件
			            System.out.println(i);
            wordgaopindipin.Add(list.get(i).getKey());
			            Writer.append(list.get(i).getKey() + " " + list.get(i).getValue() + "\r\n");
		            }
	            }

	        Writer.close();
        }

	public static void DisTfIdf(Dictionary<String, Dictionary<String, Float>> tfidf, String outpath)
{
    Iterator iter1 = tfidf.entrySet().iterator();
		while (iter1.hasNext()) {
        Dictionary.Entry entrys = (Dictionary.Entry)iter1.next();
        System.out.println("FileName: " + getFileNameWithSuffix(entrys.getKey().toString()));
        Dictionary<String, Float> temp = (Dictionary<String, Float>)entrys.getValue();
        // 将排序结果输入到文本
        Rank(temp, outpath + getFileNameWithSuffix(entrys.getKey().toString()));
        // 这里使用排序输出
    }
}

    public void main(String[] args)
    {
        // 输入目录及输出目录
        
        String inputpath = "F:\\QianYang\\Test\\";
        String outpath = "F:\\QianYang\\Test1\\";
        Dictionary<String, Dictionary<String, float>> all_tf = TfAllFiles(inputpath);
        Dictionary<String, float> idfs =Idf(all_tf);
        // System.out.println();
        tf_idf(all_tf, idfs, outpath);

    }

public static double getIMin() 
{
    CalcuDegree CD = new CalcuDegree(1);
    Dictionary<String, Double> Dictionary = new Dictionary<String, Double>();
		for (int i = 0; i<fileList.size(); i++) {
			CD.NormalTFOfAll();
			CD.tfOfAll();
			CD.tfidf();
			Dictionary.put(fileList1.get(i), calcuDegree.tfidf(fileList1.get(i)));
		}
		List<Dictionary.Entry<String, Double>> list = new List<Dictionary.Entry<String, Double>>(Dictionary.entrySet());
// 然后通过比较器来实现排序
Collections.sort(list, new Comparator<Dictionary.Entry<String, Double>>() {
			// 升序排序
			public int compare(Entry<String, Double> o1, Entry<String, Double> o2)
{
    return o1.getValue().compareTo(o2.getValue());
}

		});

		for (Dictionary.Entry<String, Double> Dictionaryping : list) {
			return Dictionaryping.getValue();

		}
		return 0;
	}

	 public static List<Term> cutWord(String file) throws IOException
{
    List<Term> termList = NLPTokenizer.segment(file);
	        return termList;
}

public static double getNImax()
{
    File file = new File("D:/workspaceMars/publicOptionMonitoring/HanLPdata/data/dictionary/CoreNatureDictionary.txt");
    BufferedReader reader = null;

    try
    {
        int totalWords = 0;
        reader = new BufferedReader(new FileReader(file));
        String tempString = null;
        int line = 1;
        // 一次读入一行，直到读入null为文件结束
        while ((tempString = reader.readLine()) != null)
        {
            if (tempString.contains("	"))
            {
                String strs[] = tempString.split("	");
                DictionaryINmax.put(strs[0], (1 * Log.log((50000) / (Double.parseDouble(strs[2])), 10) * 4));
            }
        }

        calcuDegree CD = new calcuDegree();
        Dictionary<String, Double> Dictionary = new TreeDictionary<String, Double>();


        for (int i = 0; i < fileList1.size(); i++)
        {
            Dictionary<String, Float> dict = new Dictionary<String, Float>();
            dict = calcuDegree.tf(calcuDegree.cutWord(fileList1.get(i)));
            Dictionary<String, Float> idf = calcuDegree.idf();
            double sumTfIdf = 0;
            for (String word : dict.keySet())
            {
                sumTfIdf += (idf.get(word)) * DictionaryINmax.get(word);

            }

            Dictionary.put(fileList1.get(i), sumTfIdf);
        }
        List<Dictionary.Entry<String, Double>> list = new List<Dictionary.Entry<String, Double>>(Dictionary.entrySet());
        // 然后通过比较器来实现排序
        Collections.sort(list, new Comparator<Dictionary.Entry<String, Double>>()
        {
                // 升序排序
        public int compare(Entry<String, Double> o1, Entry<String, Double> o2)
        {
            return o1.getValue().compareTo(o2.getValue());
        }

    });

    for (Dictionary.Entry<String, Double> Dictionaryping : list)
    {
        return Dictionaryping.getValue();

    }
    reader.close();
    return 0;



} catch (IOException e) {
	         e.printStackTrace();
	     } finally {
	         if (reader != null) {
	             try {
	                 reader.close();
	             } catch (IOException e1) {
	             }
	         }
	     }
		return 0;
	}

	public static double getP()
{
		return (getIMin() > getNImax()) ? getIMin() : getNImax();
}
    }
}
