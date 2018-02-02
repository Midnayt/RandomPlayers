using System;

namespace RandomPlayers.Extentions {
    public class InitializeExceptions : Exception {
        public InitializeExceptions() : base("Forms was not initialized") { }
    }

    public class UpdateContainerException : Exception {
        public UpdateContainerException() : base("Forms.UpdateContainer exception") { }

        public UpdateContainerException(string message) : base(message) { }
    }
}