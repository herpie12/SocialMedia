using CQRS.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Post.Commen.Events
{
    public class PostRemovedEvent: BaseEvent
    {
        public PostRemovedEvent(): base(nameof(PostRemovedEvent))
        {
        }
    }
}
