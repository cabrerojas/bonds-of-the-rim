namespace BondsOfTheRim
{
    public enum RelationshipScene
    {
        None,
        Romantic,
        Family,
        Friendship
    }

    public static class RelationshipScenePolicy
    {
        private const int ListenerApproachBufferTicks = 1200;

        public static RelationshipScene For(string relationDefName)
        {
            switch (relationDefName)
            {
                case "Spouse":
                case "Fiance":
                case "Lover":
                    return RelationshipScene.Romantic;
                case "Parent":
                case "Child":
                case "Sibling":
                case "HalfSibling":
                    return RelationshipScene.Family;
                case "Friend":
                    return RelationshipScene.Friendship;
                default:
                    return RelationshipScene.None;
            }
        }

        public static int DurationTicks(RelationshipScene scene)
        {
            switch (scene)
            {
                case RelationshipScene.Romantic:
                    return 420;
                case RelationshipScene.Family:
                    return 300;
                case RelationshipScene.Friendship:
                    return 240;
                default:
                    return 0;
            }
        }

        public static int ListenerDurationTicks(RelationshipScene scene)
        {
            return DurationTicks(scene) + ListenerApproachBufferTicks;
        }

        public static string StartedMessageKey(RelationshipScene scene)
        {
            switch (scene)
            {
                case RelationshipScene.Romantic:
                    return "BondsOfTheRim.Scene.RomanticStarted";
                case RelationshipScene.Family:
                    return "BondsOfTheRim.Scene.FamilyStarted";
                case RelationshipScene.Friendship:
                    return "BondsOfTheRim.Scene.FriendshipStarted";
                default:
                    return null;
            }
        }
    }
}
