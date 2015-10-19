namespace Helvetica.NntpClient.Interfaces
{
    public enum NntpResponseCode
    {
        Unknown = 0,
        //Informative Message -
        //  Connection, setup, and miscellaneous message
        HelpTextFollows = 100,
        //  Debug Output
        DebugOutput = 199,

        //Command OK -
        //  Connection, setup, and miscellaneous message
        ServerReadyPostingAllowed = 200,
        ServerReadyNoPostingAllowed = 201,
        SlaveStatusNoted = 202,
        ClosingConnection = 205,
        //  Newsgroup selection
        NflsGroupSelected = 211,
        ListOfNewsgroupsFollows = 215,
        //  ArticleId selection
        ArticleRetrievedHeadAndBodyFollows = 220,
        ArticleRetrievedHeadFollows = 221,
        ArticleRetrievedBodyFollows = 222,
        ArticleRetrievedRequestTextSeparately = 223,
        //  Distribution functions
        ListOfNewArticlesByMessageIdFollows = 230,
        ListOfNewNewsgroupsFollows = 231,
        ArticleTransferredOk = 235,
        //  Posting
        ArticlePostedOk = 240,
        //  Nonstandard (private implementation) extensions
        AuthenticationAccepted = 281,

        //Command OK so far, send the rest of it. -
        SendArticleToBeTransfered = 335,
        //  Posting
        SendArticleToBePosted = 340,
        //  Nonstandard (private implementation) extensions
        MoreAuthenticationInformationRequired = 381,

        //Command was correct, but couldn't be performed for some reason -
        ServiceDiscontinued = 400,
        //  Newsgroup selection
        NoSuchNewsGroup = 411,
        NoNewsgroupHasBeenSelected = 412,
        //  ArticleId selection
        NoCurrentArticleHasBeenSelected = 420,
        NoNextArticleInThisGroup = 421,
        NoPreviousArticleInThisGroup = 422,
        NoSuchArticleNumberInThisGroup = 423,
        //  Distribution functions
        NoSuchArticleFound = 430,
        ArticleNotWanted = 435,
        TransferFailed = 436,
        ArticleRejected = 437,
        //  Posting
        PostingNotAllowed = 440,
        PostingFailed = 441,
        //  Nonstandard (private implementation extensions
        AuthenticationRequired = 480,
        AuthenticationRejected = 482,

        //Command unimplemented, or incorrect, or a serious program error occurred.
        CommandNotRecognized = 500,
        CommandSyntaxError = 501,
        PermissionDenied = 502,
        ProgramFault = 503,

        InternalError = 603
    }
}