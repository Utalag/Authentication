namespace Authentication.Client.Pages.Sudoku
{
    public class CellModel
    {

        public CellModel()
        {
        }

        public CellModel(int row,int column)
        {
            Row = row;
            Column = column;
        }


        public int Row { get; set; }    // axis x
        public int Column { get; set; } // axis y
        public int Number { get; set; } // number

        public List<int> PossibleValues { get; set; } = new List<int>(); // possible values


        public bool IsSolid { get; set; } = false; // (Cz: pevná hodnota)
        public string IsSolidBackground { get => IsSolid == true ? "background:black;color:white" : ""; }   // gray
        public string IsSolidDisabled { get => IsSolid == true ? "disabled" : ""; } //




        public bool IsFailnigInRow { get; set; } = false; // (Cz: chyba na řádku)
        public string IsFailnigInRowBacground { get => IsFailnigInRow == true ? "background:rgb(222, 188, 17)" : ""; } //red



        public bool IsFailnigInColumn { get; set; } = false; //(Cz: chyba ve sloupci)
        public string IsFailnigInColumnBackground { get => IsFailnigInColumn == true ? "background:rgb(222, 17, 17)" : ""; } // yelow


        public bool IsFailnigInSection { get; set; } = false; //(Cz: chyba v sekci)
        public string IsFailnigInSectionBackground { get => IsFailnigInSection == true ? "background:rgb(216, 76, 228)" : ""; } // purple


        public string Background
        {
            get
            {
                return String.Format("{0};{1};{2};{3}",IsSolidBackground,IsFailnigInRowBacground,IsFailnigInColumnBackground,IsFailnigInSectionBackground);
            }

        }

        public override string? ToString()
        {
            return Number.ToString();
        }
    }
}
