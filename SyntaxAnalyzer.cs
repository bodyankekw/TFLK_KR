using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tf9ik
{
    struct findedError
    {
        public int position;
        public int stringNumber;
        public int positionString;
        public int code;
        public string value;
        public int i;
        public findedError(int code, int position, int stringNumber, int positionString, string value, int i)
        {
            this.position = position;
            this.stringNumber = stringNumber;
            this.positionString = positionString;
            this.code = code;
            this.value = value;
            this.i = i;
        }
    }

    class SyntaxAnalyzer
    {
        private List<findedError> erroors;

        public List<findedError> Errors(List<ScanResult> scanResults)
        {
            erroors = new List<findedError>();
            for (int i = 0; i < scanResults.Count;)
            {
                i = Analysis(scanResults, i) + 1;
                for (; i < scanResults.Count; i++)
                {
                    if (scanResults[i].ElementCode == 0) break;
                }
            }
            return erroors;
        }


        private int Analysis(List<ScanResult> scanResults, int i)
        {
            int state = 0;
            bool warning = true;
            if (state == 0 && scanResults[i].ElementCode != 0)
            {
                erroors.Add(new findedError(0, scanResults[i].Position, scanResults[i].NumberString, scanResults[i].PositionString, scanResults[i].Value, i));
                state = 2;
               
            }
            i++;
            for (; i < scanResults.Count; i++)
            {
                switch (state)
                {
                    case 0://name
                        if (scanResults[i].ElementCode == 1) //name 
                            state = 2;
                        else
                        {
                            state = 2;
                            erroors.Add(new findedError(-state, scanResults[i].Position, scanResults[i].NumberString, scanResults[i].PositionString, scanResults[i].Value, i));
                        }
                        break;
                    case 1://name 
                        state = 2;
                        if (scanResults[i].ElementCode != 1)
                        {
                            erroors.Add(new findedError(-state, scanResults[i].Position, scanResults[i].NumberString, scanResults[i].PositionString, scanResults[i].Value, i));
                            if (scanResults[i].ElementCode == 2) i--;
                        }
                        break;
                    case 2://As 
                        state = 3;
                        if (scanResults[i].ElementCode != 2)
                        {
                            erroors.Add(new findedError(-state, scanResults[i].Position, scanResults[i].NumberString, scanResults[i].PositionString, scanResults[i].Value, i));
                            if (scanResults[i].ElementCode == 3) i--;
                        }
                        break;
                    case 3://type 
                        state = 4;
                        if (scanResults[i].ElementCode != 3)
                        {
                            erroors.Add(new findedError(-state, scanResults[i].Position, scanResults[i].NumberString, scanResults[i].PositionString, scanResults[i].Value, i));
                            if (scanResults[i].ElementCode == 4) i--;
                        }
                        break;
                    case 4://'=' or ',' or '/n' 
                        if (scanResults[i].ElementCode == 4)
                        {
                            state = 5;
                        }
                        else if (scanResults[i].ElementCode == 7)
                        {
                           state = 8;
                        }
                        else if (scanResults[i].ElementCode == 6)
                        {
                           state = 7;
                        }
                        else
                        {
                            state = 8;
                            erroors.Add(new findedError(-state, scanResults[i].Position, scanResults[i].NumberString, scanResults[i].PositionString, scanResults[i].Value, i));
                            if (scanResults[i].ElementCode == 0) i--;
                        }
                        break;
                    case 5://const 
                        state = 6;
                        if (scanResults[i].ElementCode != 5)
                        {
                            erroors.Add(new findedError(-state, scanResults[i].Position, scanResults[i].NumberString, scanResults[i].PositionString, scanResults[i].Value, i));
                            if (scanResults[i].ElementCode == 6 || scanResults[i].ElementCode == 7) i--;
                        }
                        break;
                    case 6://',' or '/n' 
                        if (scanResults[i].ElementCode == 6)
                        {
                            state = 7;
                        }
                        else if (scanResults[i].ElementCode == 7)
                        {
                            
                            state = 8;
                        }
                        else
                        {
                            state = 7;
                            erroors.Add(new findedError(-state, scanResults[i].Position, scanResults[i].NumberString, scanResults[i].PositionString, scanResults[i].Value, i));
                            if (scanResults[i].ElementCode == 1 || scanResults[i].ElementCode == 5) i--;
                        }
                        break;
                    case 7://name 
                        if (scanResults[i].ElementCode == 1)
                        {
                            state = 2;
                        }
                        else if (scanResults[i].ElementCode == 5)
                        {
                            double num = System.Convert.ToDouble(scanResults[i - 2].Value);
                            if (num % 1 > 0 && warning == false)
                                state = -8;
                            else
                            {
                                state = 6;
                                erroors.Add(new findedError(1, scanResults[i].Position, scanResults[i].NumberString, scanResults[i].PositionString, "вместо `.` встречено `,`", i));
                            }
                        }
                        else
                        {
                            state = 8;
                            erroors.Add(new findedError(-state, scanResults[i].Position, scanResults[i].NumberString, scanResults[i].PositionString, scanResults[i].Value, i));
                            if (scanResults[i].ElementCode == 0 || scanResults[i].ElementCode == 7) i--;
                        }
                        break;
                    case 8://end 
                        if (scanResults[i].ElementCode == 0)
                        {
                            state = 0;
                        }
                        else if (scanResults[i].ElementCode == 7)
                        {
                            state = 8;
                        }
                        else
                        {
                            state = 6;
                            erroors.Add(new findedError(-state, scanResults[i].Position, scanResults[i].NumberString, scanResults[i].PositionString, scanResults[i].Value, i));
                            if (scanResults[i].ElementCode == 6 || scanResults[i].ElementCode == 7) i--;
                        }
                        break;
                    default:
                        break;
                }
            }
            if (state == 4 || state == 6)
            {
                return i;
            }
            else
            {
                erroors.Add(new findedError(-state - 1, scanResults[i - 1].Position, scanResults[i - 1].NumberString, scanResults[i - 1].PositionString, "пусто", i - 1));
                return i - 1;
            }
        }
    }
}
