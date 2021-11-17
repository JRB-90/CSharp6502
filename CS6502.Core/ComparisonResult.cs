namespace CS6502.Core
{
    /// <summary>
    /// Holds the output of a comparisson between teo cycle states.
    /// </summary>
    public class ComparisonResult
    {
        public ComparisonResult(
            int cycleID,
            bool rw_Matches,
            bool a_matches,
            bool x_matches,
            bool y_matches,
            bool ir_matches,
            bool p_matches,
            bool sp_matches,
            bool pc_matches,
            bool address_matches,
            bool data_matches)
        {
            CycleID = cycleID; ;
            RW_Matches = rw_Matches;
            A_Matches = a_matches;
            X_Matches = x_matches;
            Y_Matches = y_matches;
            IR_Matches = ir_matches;
            P_Matches = p_matches;
            SP_Matches = sp_matches;
            PC_Matches = pc_matches;
            Address_Matches = address_matches;
            Data_Matches = data_matches;
        }
        
        public int CycleID { get; }

        public bool RW_Matches { get; }

        public bool A_Matches { get; }

        public bool X_Matches { get; }

        public bool Y_Matches { get; }

        public bool IR_Matches { get; }

        public bool P_Matches { get; }

        public bool SP_Matches { get; }

        public bool PC_Matches { get; }

        public bool Address_Matches { get; }

        public bool Data_Matches { get; }

        public bool IsCompleteMatch
        {
            get
            {
                return
                    RW_Matches &&
                    A_Matches &&
                    X_Matches &&
                    Y_Matches &&
                    IR_Matches &&
                    P_Matches &&
                    SP_Matches &&
                    PC_Matches &&
                    Address_Matches &&
                    Data_Matches;
            }
        }

        public override string ToString()
        {
            return
                $"{CycleID}\t" +
                $"{RW_Matches}\t" +
                $"{A_Matches}\t" +
                $"{X_Matches}\t" +
                $"{Y_Matches}\t" +
                $"{IR_Matches}\t" +
                $"{P_Matches}\t" +
                $"{SP_Matches}\t" +
                $"{PC_Matches}\t" +
                $"{Address_Matches}\t" +
                $"{Data_Matches}";
        }
    }
}
