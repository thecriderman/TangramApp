using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TangramApp1._35.Controls.Events;

namespace TangramApp1._35.Controls
{
    /// <summary>
    /// Much more classical interface usage for events (JAVALIKE).
    /// IT does in fact have a use.
    /// </summary>
    public interface IVotableHandler
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        bool RequestVote(VotableTouchButton2 v);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="v"></param>
        /// <returns>True if the button's vote was unanimous.</returns>
        bool CastVote(VotableTouchButton2 v);
        bool CancelVote(VotableTouchButton2 v);

        /// <summary>
        /// Necessary for exitcancelbutton
        /// </summary>
        event VotableButtonTrayChangedEventHandler VoteTargetChanged;
    }
}
