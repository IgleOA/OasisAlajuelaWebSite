using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using ET;

namespace BL
{
    public class TokensBL
    {
        private TokensDAL TDAL = new TokensDAL();

        public bool AddNew(Token details)
        {
            return TDAL.AddNew(details);
        }

        public Token ValidateToken(string tokenID)
        {
            return TDAL.ValidateToken(tokenID);
        }
    }
}
