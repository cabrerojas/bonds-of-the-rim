using System;

namespace BondsOfTheRim.Tests
{
    internal static class InvitationPolicyTests
    {
        private static int Main()
        {
            AssertRelationship("Parent", true);
            AssertRelationship("Child", true);
            AssertRelationship("Sibling", true);
            AssertRelationship("HalfSibling", true);
            AssertRelationship("Spouse", true);
            AssertRelationship("Fiance", true);
            AssertRelationship("Lover", true);
            AssertRelationship("Friend", true);
            AssertRelationship("Acquaintance", false);
            AssertRelationship("Grandparent", false);

            AssertEqual(0f, InvitationPolicy.ClampChance(-0.25f), "ClampChance clamps negative values");
            AssertEqual(0.6f, InvitationPolicy.ClampChance(0.6f), "ClampChance preserves valid values");
            AssertEqual(1f, InvitationPolicy.ClampChance(1.25f), "ClampChance clamps values over one");

            AssertEqual(RelationshipScene.Romantic, RelationshipScenePolicy.For("Lover"), "Lover selects romantic scene");
            AssertEqual(RelationshipScene.Romantic, RelationshipScenePolicy.For("Spouse"), "Spouse selects romantic scene");
            AssertEqual(RelationshipScene.Family, RelationshipScenePolicy.For("Parent"), "Parent selects family scene");
            AssertEqual(RelationshipScene.Family, RelationshipScenePolicy.For("Sibling"), "Sibling selects family scene");
            AssertEqual(RelationshipScene.Friendship, RelationshipScenePolicy.For("Friend"), "Friend selects friendship scene");
            AssertEqual(RelationshipScene.None, RelationshipScenePolicy.For("Acquaintance"), "Unsupported relation has no scene");
            AssertEqual(420, RelationshipScenePolicy.DurationTicks(RelationshipScene.Romantic), "Romantic scene duration");
            AssertEqual(300, RelationshipScenePolicy.DurationTicks(RelationshipScene.Family), "Family scene duration");
            AssertEqual(240, RelationshipScenePolicy.DurationTicks(RelationshipScene.Friendship), "Friendship scene duration");
            AssertEqual(1620, RelationshipScenePolicy.ListenerDurationTicks(RelationshipScene.Romantic), "Romantic listener duration includes approach time");
            AssertEqual(1500, RelationshipScenePolicy.ListenerDurationTicks(RelationshipScene.Family), "Family listener duration includes approach time");
            AssertEqual(1440, RelationshipScenePolicy.ListenerDurationTicks(RelationshipScene.Friendship), "Friendship listener duration includes approach time");

            AssertEqual(true, InvitationCooldownPolicy.IsAvailable(600000, 600000), "Cooldown expires exactly at its end tick");
            AssertEqual(false, InvitationCooldownPolicy.IsAvailable(599999, 600000), "Cooldown blocks before its end tick");
            AssertEqual(15f, InvitationCooldownPolicy.RemainingDays(0, 900000), "Cooldown lasts fifteen in-game days");
            AssertEqual(0f, InvitationCooldownPolicy.RemainingDays(900000, 900000), "Expired cooldown has no remaining days");

            Console.WriteLine("All InvitationPolicy checks passed.");
            return 0;
        }

        private static void AssertRelationship(string relationDefName, bool expected)
        {
            AssertEqual(expected, InvitationPolicy.IsEligibleRelationship(relationDefName), relationDefName);
        }

        private static void AssertEqual<T>(T expected, T actual, string description)
        {
            if (!Equals(expected, actual))
            {
                throw new InvalidOperationException(
                    $"{description}: expected '{expected}', received '{actual}'.");
            }
        }
    }
}
