namespace BondsOfTheRim
{
    public static class InvitationPolicy
    {
        public const int FriendOpinionThreshold = 60;

        public static bool IsEligibleRelationship(string relationDefName)
        {
            switch (relationDefName)
            {
                case "Parent":
                case "Child":
                case "Sibling":
                case "HalfSibling":
                case "Spouse":
                case "Fiance":
                case "Lover":
                case "Friend":
                    return true;
                default:
                    return false;
            }
        }

        public static float CalculateAcceptanceChance(string relationDefName, int socialSkill, int visitorOpinion)
        {
            var relationshipChance = relationDefName == "Friend" ? 0.20f : 0.35f;
            var socialBonus = socialSkill * 0.015f;
            var opinionBonus = visitorOpinion * 0.003f;
            return ClampChance(relationshipChance + socialBonus + opinionBonus);
        }

        public static float ClampChance(float chance)
        {
            if (chance < 0f)
            {
                return 0f;
            }

            return chance > 1f ? 1f : chance;
        }
    }
}
