using System;

namespace kOS.Safe.Encapsulation.Suffixes
{
    public class SevenArgsSuffix<TReturn, TParam, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7> : SuffixBase
        where TReturn : Structure where TParam : Structure where TParam2 : Structure where TParam3 : Structure where TParam4 : Structure where TParam5 : Structure where TParam6 : Structure where TParam7 : Structure
    {
        private readonly Del<TReturn, TParam, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7> del;

        public delegate TInnerReturn Del<out TInnerReturn, in TInnerParam, in TInnerParam2, in TInnerParam3, in TInnerParam4, in TInnerParam5, in TInnerParam6, in TInnerParam7>(TInnerParam one, TInnerParam2 two, TInnerParam3 three, TInnerParam4 four, TInnerParam5 five, TInnerParam6 six, TInnerParam7 seven);

        public SevenArgsSuffix(Del<TReturn, TParam, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7> del, string description = "")
            : base(description)
        {
            this.del = del;
        }

        protected override object Call(object[] args)
        {
            return (TReturn)del((TParam)args[0], (TParam2)args[1], (TParam3)args[2], (TParam4)args[3], (TParam5)args[4], (TParam6)args[5], (TParam7)args[6]);
        }

        protected override Delegate Delegate
        {
            get
            {
                return del;
            }
        }
    }

    public class SevenArgsSuffix<TParam, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7> : SuffixBase where TParam : Structure where TParam2 : Structure where TParam3: Structure where TParam4 : Structure where TParam5 : Structure where TParam6 : Structure where TParam7 : Structure
    {
        private readonly Del<TParam, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7> del;

        public delegate void Del<in TInnerParam, in TInnerParam2, in TInnerParam3, in TInnerParam4, in TInnerParam5, in TInnerParam6, in TInnerParam7>(TInnerParam one, TInnerParam2 two, TInnerParam3 three, TInnerParam4 four, TInnerParam5 five, TInnerParam6 six,  TInnerParam7 seven);

        public SevenArgsSuffix(Del<TParam, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7> del, string description = "")
            : base(description)
        {
            this.del = del;
        }

        protected override object Call(object[] args)
        {
            del((TParam)args[0], (TParam2)args[1], (TParam3)args[2], (TParam4)args[3], (TParam5)args[4], (TParam6)args[5], (TParam7)args[6]);
            return null;
        }

        protected override Delegate Delegate
        {
            get
            {
                return del;
            }
        }
    }
}
