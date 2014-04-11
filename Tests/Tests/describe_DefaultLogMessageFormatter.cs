using NSpec;
using NLog;

class describe_DefaultLogMessageFormatter : nspec {
    void when_created() {
        it["formats string"] = () => {
            var f = new DefaultLogMessageFormatter();
            var logger = new Logger("MyLogger");
            var message = f.FormatMessage(logger, LogLevel.Info, "hi");
            message.should_be("[INFO] MyLogger: hi");
        };
    }
}

