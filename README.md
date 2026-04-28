# Networked Card Game
**Unity · C# · Mirror Networking · Server-Authoritative Multiplayer**

A two-player networked card game built on Mirror, implementing server-authoritative deal/play flow, authority-gated player actions, and hidden-information card state across clients.

---

## Overview

The focus here is correctness of networked state rather than game complexity. Card ownership, visibility, and play actions are all server-authoritative — clients request actions, the server validates and executes, then syncs state to all connected players.

---

## Architecture

```
PlayerManager (NetworkBehaviour — server authority)
├── [Command] CmdDealCards()       — client requests deal, server executes
├── [Command] CmdPlayCard()        — client requests play, server validates
├── [ClientRpc] RpcUpdateHand()    — server pushes hand state to all clients
└── [SyncVar] currentTurn          — replicated turn state

Card Visibility
└── CardFlipper.cs — toggles Sprite between face/back based on hasAuthority
    ├── Own cards   → face visible
    └── Opponent    → back visible (hidden information)

Card Lifecycle
├── Server: Instantiate + NetworkServer.Spawn
└── Client: Reparent into player area / opponent area / table zone
```

---

## Key Design Decisions

**Server-authoritative deal and play** — All card operations originate as `[Command]` calls from the client and execute on the server. Clients never directly manipulate card state.

**Authority-gated input** — `hasAuthority` guards all player interaction. A client can only interact with objects it owns — opponent cards and out-of-turn actions are blocked at the component level.

**Hidden information via sprite swap** — `CardFlipper` checks authority on each card instance. Own cards show face; opponent cards show back. State is consistent across both clients without a separate visibility layer.

**Server spawn + client reparent** — Cards are spawned on the server via `NetworkServer.Spawn`, then reparented on each client into the correct UI zone (hand, table, opponent area) based on the receiving player's context.

---

## What This Demonstrates

- Mirror `NetworkBehaviour` with `[Command]` / `[ClientRpc]` / `[SyncVar]`
- Server-authoritative game state management
- Authority-based input gating (`hasAuthority`)
- Hidden information handling in a networked context
- Server-side spawn with client-side UI placement

---

## Tech

- Unity 2020.3 · C#
- Mirror (vendored)
- TextMeshPro
