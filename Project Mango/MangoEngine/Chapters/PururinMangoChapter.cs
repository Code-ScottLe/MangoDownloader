﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using MangoEngine.Exceptions;
using HtmlAgilityPack;

namespace MangoEngine.Chapters
{
    public class PururinMangoChapter : MangoChapter
    {
        #region Fields
        /*Fields*/
        #endregion

        #region Properties
        /*Properties*/
        #endregion

        #region Constructors
        /*Constructors*/
        #endregion

        #region Methods
        /*Methods*/
        internal override void Init()
        {
            throw new NotImplementedException();
        }

        internal override Task InitAsync()
        {
            throw new NotImplementedException();
        }
        public override string GetImageUrl()
        {
            throw new NotImplementedException();
        }

        public override Task<string> GetImageUrlAsync()
        {
            throw new NotImplementedException();
        }

        public override bool NextPage()
        {
            throw new NotImplementedException();
        }

        public override Task<bool> NextPageAsync()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
