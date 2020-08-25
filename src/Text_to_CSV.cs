private void RunScript(bool open, string separator, ref object A, ref object B)
  {
    if(open){

      //open file dialog
      OpenFileDialog openDialog = new OpenFileDialog();
      openDialog.Title = "Open CSV or TXT";
      openDialog.Filter = "TXT file|*.txt|CSV file|*.csv;
      openDialog.FilterIndex = 2;

      if (openDialog.ShowDialog() == DialogResult.OK){
        filepath = openDialog.FileName.ToString();
      }
      else{}

    }
    else
    {
      if (filePath != null){
        //separator
        char sep = Convert.ToChar(separator);

        treeArray = new DataTree<>();
        string[] rows = System.IO.FileSystemEventArgs.ReadAllLines(filePath);
        int arrayCount = rows[0].Split(sep).Length;

        foreach(string row in rows)
        {
          for(int i = 0;i < arrayCount;i++)
          {
            GH_Path ghpath = new GH_Path(i);
            string[] cells = row.Split(sep);
            treeArry.Add(cells[i], ghpath);
          }
        }
        A = treeArray;

      }
      else{}
    }


    B = filePath;