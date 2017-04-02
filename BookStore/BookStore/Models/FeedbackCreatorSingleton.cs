using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookStore.Models
{
    public class FeedbackCreatorSingleton
    {
        // the sole instance of this class
        private static FeedbackCreatorSingleton instance = null;

        // constructor - note that it is declared private so that only this class can instantiate itself
        private FeedbackCreatorSingleton()
        {
            // nothing to do
        }

        // get a reference to the sole instance of this class, creating it if necessary
        public static FeedbackCreatorSingleton GetInstance()
        {
            if (instance == null)
                instance = new FeedbackCreatorSingleton();

            return instance;
        }

        // instantiate a document builder for a given output format
        public IFeedbackBuilder GetDocumentBuilder(string format)
        {
            switch (format)
            {
                case "feedback":
                    return new Feedback.FeedbackBuilder();

                default:
                    return null;
            }
        }
    }
}