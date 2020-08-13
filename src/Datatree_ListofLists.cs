public static List<List<T>> ToListOfLists<T>(DataTree<T> tree)
        {
            List<List<T>> list = new List<List<T>>();
            foreach (List<T> b in tree.Branches)
            {
                list.Add(b);
            }
            return list;
        }
