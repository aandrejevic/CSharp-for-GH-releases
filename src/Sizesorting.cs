  private void RunScript(List<double> dList, object y, ref object countList)
  {
    List<int> count = new List<int>();

    HashSet<double> hs = new HashSet<double>();

    int counter = 1;
    for(int i = 1; i < dList.Count;i++){
      if(dList[i - 1] == dList[i]) counter++;
      else{ count.Add(counter); counter = 1;}
      if(i == dList.Count - 1) count.Add(counter);
    }
    countList = count;
  }