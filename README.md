# Bonds of the Rim

A small RimWorld 1.6 mod: a colonist can ask a close relative, partner, or trusted friend from a non-hostile outside faction to join the colony.

## MVP rules

- The action appears when one selected, player-controlled colonist right-clicks an eligible visitor.
- Eligible direct relations: parent, child, sibling, half-sibling, spouse, fiance, lover, or friend with at least +60 opinion.
- The visitor must be alive, humanlike, spawned, non-hostile, unjailed, and belong to a non-player faction.
- Acceptance chance depends on the relationship, the colonist's Social skill, and the visitor's opinion.
- Every attempt applies a 10-day cooldown to that visitor, whether it succeeds or fails.
- The MVP intentionally does not recruit raiders, prisoners, animals, or arbitrary guests.

## Local development

Develop here; do not edit the Steam-managed RimWorld files. Build against your installed game and copy or symlink this whole folder into `RimWorld/Mods/BondsOfTheRim` for local testing.

The project defaults to the Windows Steam location. Override it when needed:

```powershell
dotnet build Source/BondsOfTheRim/BondsOfTheRim.csproj `
  -p:RimWorldManagedPath='C:\path\to\RimWorld\RimWorldWin64_Data\Managed'
```

Run the dependency-free policy checks with:

```powershell
dotnet run --project Source/BondsOfTheRim.Tests/BondsOfTheRim.Tests.csproj
```

The checks intentionally use no NuGet packages, so they run without network package feeds. The repository needs a .NET SDK capable of building `net472` for the game assembly and `net8.0` for the policy checks. The current WSL environment has no `dotnet` executable, so compile and in-game verification remain pending.
