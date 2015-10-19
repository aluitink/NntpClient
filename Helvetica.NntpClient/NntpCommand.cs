using System;

namespace Helvetica.NntpClient
{
    public class NntpCommand
    {

        #region NNTP Base Commands
        //ARTICLE [article number|<message-id>]
        //param: an article number
        //param: a Message ID
        //Retrieve article
        /*
         * Tells the server to send the client a particular Usenet article. 
         * The article to be retrieved may be specified either using its absolute, 
         * universal message ID, or its locally-assigned article number.
         * When the command is issued with an article number, 
         * this causes the server's internal message pointer to be set to the specified 
         * article. If the message pointer is already set to a particular article, 
         * the ARTICLE command can be issued without an article number and the 
         * current message will be retrieved.
         */
        public const String Article = "ARTICLE";

        //BODY [article number|<message-id>]
        //param: an article number
        //param: a Message ID
        //Retrieve article body
        /*
         * Same as the ARTICLE command, but retrieves only the article's headers.
         */
        public const String Body = "BODY";

        //HEAD [article number|<message-id>]
        //param: an article number
        //param: a Message ID
        //Retrieve article header
        /*
         * Same as the ARTICLE command, but returns only the body of the article.
         */
        public const String Head = "HEAD";

        //STAT [article number]
        //param: an article number
        //Sets internal article pointer
        /*
         * Conceptually the same as the ARTICLE command, but does not return any message text, 
         * only the message ID of the article. 
         * This command is usually used for the purpose of setting the server's internal message pointer, 
         * so STAT is normally invoked only with an article number (and not a message ID).
         */
        public const String Stat = "STAT";

        //GROUP [newsgroup name]
        //param: a newsgroup name
        //
        /*
         * Tells the server the name of the newsgroup that the client wants to access. 
         * Assuming the group specified exists, the server returns to the client 
         * the numbers of the first and last articles currently in the group, 
         * along with an estimate of the number of messages in the group. 
         * The server's internal article pointer is also set to the first message in the group.
         */
        public const String Group = "GROUP";

        //HELP
        /*
         * Prompts the server to send the client help information, which usually takes the form 
         * of a list of valid commands that the server supports.
         */
        public const String Help = "HELP";

        //IHAVE [message-id]
        //param: Message ID
        /*
         * Used by the client in an NNTP session to tell the server that it has a new article 
         * that the server may want. The server will check the message ID provided and respond 
         * to the client indicating whether or not it wants the client to send the article.
         */
        public const String Have = "IHAVE";

        //LAST
        /*
         * Tells the server to set its current article pointer to the last message in the newsgroup.
         */
        public const String Last = "LAST";

        //LIST
        /*
         * Asks the server to send a list of the newsgroups that it supports, 
         * along with the first and last article number in each group. 
         * The command as described in RFC 977 is simple, supporting no parameters and causing the full list of newsgroups to be sent to the client. 
         * NNTP command extensions significantly expand the syntax of this command.
         */
        public const String List = "LIST";

        //NEWGROUPS [datetime|[distribution specification]]
        //param: Date and time, and optional distribution specification
        /*
         * Prompts the server to send a list of new newsgroups created since the date and time specified. 
         * The client may also restrict the command to return only new newsgroups within a particular regional distribution.
         */
        public const String Newsgroup = "NEWSGROUP";

        //NEWNEWS [datetime|[distribution specification]]
        //param: Date and time, and optional distribution specification
        /*
         * Requests a list from the server of all new articles that have arrived since a particular date and time. 
         * Like the NEWGROUPS command, this may be restricted in distribution. 
         * The server responds with a list of message IDs of new articles.
         */
        public const String NewNews = "NEWNEWS";

        //NEXT
        /*
         * Advances the server's current article pointer to the next message in the newsgroup.
         */
        public const String Next = "NEXT";

        //POST
        /*
         * Tells the server that the client would like to post a new article. 
         * The server responds with either a positive or negative acknowledgment. 
         * Assuming that posting is allowed, the client then sends the full text 
         * of the message to the server, which stores it and begins the process 
         * of propagating it to other servers.
         */
        public const String Post = "POST";

        //QUIT
        /*
         *Terminates the NNTP session. 
         *To be “polite”, the client should issue this command prior to closing the TCP connection.
         */
        public const String Quit = "QUIT";

        //SLAVE
        /*
         * This command is intended for use in special configurations where 
         * one NNTP server acts as a subsidiary to others. 
         * It is not often used in practice.
         */
        public const String Slave = "SLAVE";
        #endregion

        #region NNTP Transport Extensions
        //MODE STREAM
        /*
         * Used to tell the server that the client wants to operate in stream mode, 
         * using the CHECK and TAKETHIS commands.
         */
        public const String ModeStream = "MODE STREAM";

        //CHECK [message-id]
        //param: Message ID
        /*
         * Used in stream mode by a server acting as a client to ask another server 
         * if it has a copy of a particular article. 
         * The server responds back indicating whether or not it wishes to be sent a copy of the article. 
         * This command is similar to IHAVE, except that the client does not have to 
         * wait for a reply before sending the next command.
         */
        public const String Check = "CHECK";

        //TAKETHIS [message-id]
        //param: Message ID
        /*
         * When a server responds to a CHECK command indicating that it wants a copy of a particular message, 
         * the client sends it using this command.
         */
        public const String TakeThis = "TAKETHIS";

        //XREPLIC [list of newsgroups and article numbers]
        /*
         * This command was created for the special purpose of copying large numbers of articles from one server to another. 
         * It is not widely used.
         */
        public const String XReplic = "XREPLIC";
        #endregion

        #region NNTP LIST Command Extensions
        //LIST ACTIVE [newsgroup name|pattern]
        //param: Newsgroup name
        //param: pattern
        /*
         * Provides a list of active newsgroups on the server. 
         * This is semantically the same as the original LIST command, 
         * but the client may provide a newsgroup name or a pattern to restrict 
         * the number of newsgroups returned.
         * For example, the client can ask for a list of only the newsgroups 
         * that contain “football” in them.
         */
        public const String ListActive = "LIST ACTIVE";
        //LIST ACTIVE.TIMES
        /*
         * Prompts the server to send the client its active.times file, 
         * which contains information about when the newsgroups carried 
         * by the server were created.
         */
        public const String ListActiveTimes = "LIST ACTIVE.TIMES";
        //LIST DISTRIBUTIONS
        /*
         * Causes the server to sent the client the contents of the distributions file, 
         * which shows what regional distribution strings the server recognizes 
         * (for use in the Distribution header of a message).
         */
        public const String ListDistributions = "LIST DISTRIBUTIONS";
        //LIST DISTRIB.PATS
        /*
         * Asks the server for its distribution.pats file, which is like the distributions 
         * file but uses patterns to summarize distribution information for different newsgroups.
         */
        public const String ListDistributionPatterns = "LIST DISTRIB.PATS";
        //LIST NEWSGROUPS [newsgroup name|pattern]
        //param: Newsgroup name
        //param: pattern
        /*
         * Provides a list of newsgroup names and descriptions. 
         * This differs from LIST ACTIVE in that only the newsgroup name and description are returned, 
         * and not the article numbers for each newsgroup. 
         * It is functionally the same as XGTITLE and is usually employed by a user to 
         * locate a newsgroup to be added to his or her subscribed list.
         */
        public const String ListNewsgroups = "LIST NEWSGROUPS";
        //LIST OVERVIEW.FMT
        /*
         * Prompts the server to display information about the format of its overview file. 
         * See the XOVER command description below for more.
         */
        public const String ListOverviewFormat = "LIST OVERVIEW.FMT";
        //LIST SUBSCRIPTIONS
        /*
         * Asks the server to send the client a default list of subscribed newsgroups. 
         * This is used to set up a new user with a suggested list of newsgroups. 
         * For example, if an organization has an internal support newsgroup, 
         * they could put this group on the default subscription list so all new users 
         * learn about it immediately when they first start up their newsreader.
         */
        public const String ListSubscriptions = "LIST SUBSCRIPTIONS";
        #endregion

        #region NNTP Newsreader Extensions
        //LISTGROUP [newsgroup name]
        //param: Newsgroup name
        /*
         * Causes the server to return a list of local article numbers for the current messages in the newsgroup. 
         * The server's current article pointer is also set to the first message in the group.
         */
        public const String ListGroup = "LISTGROUP";
        //MODE READER
        /*
         * Tells the server that the device acting as a client is in fact a client newsreader and not another NNTP server. 
         * While technically not required—all commands can be sent by any device acting as 
         * client—some servers may be optimized to respond to newsreader-oriented commands if given this command.
         */
        public const String ModeReader = "MODE READER";
        //XGTITLE [newsgroup name|pattern]
        //param: Newgroup name
        //param: Pattern
        /*
         * Used to list the descriptions for a newsgroup or a set of newsgroups matching a particular text pattern. 
         * This command is functionally the same as the LIST NEWSGROUP command extension. 
         * It is therefore recommended that XGTITLE no longer be used.
         */
        public const String XGTitle = "XGTITLE";

        //XHDR header [range|<message-id>]
        //param: an article number
        //param: an article number followed by a dash to indicate all following
        //param: an article number followed by a dash followed by another article number
        //Responses:
        //221 Header follows
        //412 No news group currently selected
        //420 No current article selected
        //430 no such article
        //502 no permission
        /*
         * Allows a client to ask for only a particular header from a set of messages. 
         * If only the header name is provided, the header is returned for all messages in the current group. 
         * Otherwise, the header is provided for the selected messages.
         * This extension provides a newsreader client with a more efficient way of 
         * retrieving and displaying important headers in a newsgroup to a user.
         */
        public const String XHdr = "XHDR";

        //XINDEX [newsgroup name]
        //param: Newsgroup name
        /*
         * Retrieves an index file, used by the newsreader TIN to improve the efficiency of newsgroup perusal. 
         * TIN now supports the more common “overview” format, so the XOVER command is preferred to this one.
         */
        public const String XIndex = "XINDEX";

        //XOVER [range|<message-id>]
        //param: an article number
        //param: an article number followed by a dash to indicate all following
        //param: an article number followed by a dash followed by another article number
        /*
         * Retrieves the overview for an article or set of articles. Servers supporting this feature maintain a 
         * special database for their newsgroups that contains information about current articles in a format 
         * that can be used by a variety of newsreaders. 
         * Retrieving the overview information allows features like message threading to be performed more 
         * quickly than if the client had to retrieve the headers of each message and analyze them manually.
         */
        public const String XOver = "XOVER";
        //XPAT header pattern [range|message-id]
        //param: Header name
        //param: Pattern
        //param: an article number
        //param: an article number followed by a dash to indicate all following
        //param: an article number followed by a dash followed by another article number
        /*
         * This command is similar to XHDR in that it allows a particular header to be retrieved for a set of messages. 
         * The difference is that the client can specify a pattern that must be matched for the header to be retrieved. 
         * This allows the client to have the server search for and return certain messages, such as those with a subject 
         * line indicating a particular type of discussion, rather than requiring the client to download all the headers 
         * and search through them.
         */
        public const String XPat = "XPAT";
        //XPATH [message-id]
        //param: Message ID
        /*
         * Allows a client to ask for the name of the actual file in which a particular message is stored on the server.
         */
        public const String XPath = "XPATH";
        //XROVER [range|<message-id>]
        //param: an article number
        //param: an article number followed by a dash to indicate all following
        //param: an article number followed by a dash followed by another article number
        /*
         * Like the XOVER command, but specifically retrieves information in the References header for the indicated articles. 
         * This is, of course, the header containing the data needed to create threaded conversations.
         */
        public const String XROver = "XROVER";
        //XTHREAD [dbint]
        //param: optional dbint
        /*
         * Similar to XINDEX, but retrieves a special threading information file in the format used by the newsreader TRN. 
         * Like TIN, TRN now supports the common “overview” format so XOVER is preferred to this command.
         */
        public const String XThread = "XTHREAD";
        #endregion

        #region Other
        public const String AuthUser = "AUTHINFO USER";
        public const String AuthPass = "AUTHINFO PASS";
        public const String Capabilities = "CAPABILITIES";
        #endregion
    }
}