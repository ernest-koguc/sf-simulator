# Parser fields and Rust reference

The parsers retain values read by the UI, daily completion logic, simulation requests, or statistics. Merely assigning a value to session state or writing it to an otherwise unread database property does not count as a consumer.

Offsets below are zero based integer words, not bytes. The local `parser-reference` snapshot is the reference; this audit does not claim compatibility with server revisions beyond that snapshot.

## Retained fields

| TS parser / output | Rust source and encoding | Consumer |
| --- | --- | --- |
| Character `ID` | `gamestate/mod.rs`, `ownplayersavecharacter[1]` | Account/session identity |
| Character `Level` | Same key, `[3] & 0xffff` | Combat, feature availability |
| Character `Class`; looked-up class | `ownplayersavecharacter[20]`; `OtherPlayer::update` in `gamestate/social.rs` uses the same offset | Combat, equipment export |
| Character `Armor` | `ownplayersavecharacter[23]` | Simulation request |
| Character `GuildPortal` | Same key, `[26]`, guild portal damage percentage | Simulation request |
| Character `PortalHealthBonus` | Same key, `[28]`, personal portal HP percentage | HP calculation; this is not light dungeon slot 17 |
| Character `Strength`, `Dexterity`, `Intelligence`, `Constitution`, `Luck` | Same key, bases `[30..34]` plus additions `[35..39]` | Combat |
| Status `MaxBeers`, `ThirstLeft`, `UsedBeers` | `gamestate/mod.rs`, `characterstatus[5]`, `[6]`, `[7]` | Daily thirst completion |
| Eternal life potion flag | `items::parse_active_potions`: lists shorter than 10 words are empty; potion types `[1..3]`, ID 16 is EternalLife | HP calculation |
| Wheel spins | `gamestate/mod.rs`, `wheel[1]` | Dailies |
| Dice games remaining | Same file, `dice[1]` | Dailies |
| Arena fights today | Same file, `arena[1]` | Dailies |
| Hydra attack availability | Same file, `charactergroup[3] > 0` | Dailies |
| Companion `Level` and five attributes | Same file, `owntower`: start `3 + 148*i`, level at start, five attributes at start + 4 | Companion combat; old extra attribute blocks are not added |
| Underworld `Gladiator` | `Underworld::update` / `UnderworldBuildingType`, `owntower[448 + 6]` | Crit damage |
| Underworld `TimeMachineThirst` | `Underworld::update`, `owntower[473]` | Daily Adventuromatic collection |
| Five pet arena flags | `Pets::update`, `ownpets[223..227] == 1`, Shadow / Light / Earth / Fire / Water | Dailies |
| Witch `Scrolls` | `Witch::update`, count `witchshop[4]`, IDs `witchshop[6 + 3*i]` | Missing scroll warnings; listed IDs unlock scrolls, and updates preserve earlier unlocks |
| Guild `PetMaxLevel` | `Guild::update_group_save`, `owngroupsave[378]` | Hydra daily availability |
| Portal `Finished`, `CanAttack` | `Portal::update`, `portalprogress[0]`, `portalprogress[2] != current server ordinal` | Dailies |
| Active events | `gamestate/mod.rs`, `events[1]` flags, minimum 8 fields; enum order in `gamestate/rewards.rs`; captured `events[11]` string code `hv` adds Hellevator | Session events and Tidy Toilet Time / Lucky Day rules |
| Daily reward `Claimed`, `PointsRequired` | `RewardChest::parse` / `parse_rewards`: three variable records, header `opened != 0 / points / pair count` | Daily task completion |
| Daily task `Completed`, `Points` | `dailytasklist`: skip one word, then complete 4-word records; `Task::is_completed` compares current >= target | Daily task completion |
| Toilet sacrifices remaining | `gamestate/mod.rs`, `toiletstate[2]`; ignore packets shorter than 3 words | Dailies |
| Dungeon lists / special progress | `LightDungeon`, `ShadowDungeon`, `Dungeons::update_progress` in `gamestate/dungeons.rs` | Simulation request; see representation below |
| Item `Type` | `ItemType::parse`, word `[0] & 0xff` | Slot presence, weapon/shield handling, equipment export |
| Item `Armor` | `Item::armor`, word `[5]` only for armor/accessory types 3..10 | Companion armor and equipment export |
| Item `DamageMin`, `DamageMax` | `ItemType::Weapon`, words `[5]`, `[6]` only for type 1 | Combat and equipment export |
| Item `HasEnchantment` | `Item::parse`, recognized `Enchantment` IDs at `[2]` | Crit/reaction and missing scroll warnings |
| Item `RuneType`, `RuneValue` | `Item::parse`: inspect all three type/value pairs `[7+i]` / `[10+i]`; valid rune IDs 31..42, u8 value | Damage, HP and resistance bonuses |
| Item `ItemQuality` | `Item::parse`, `[17]` | Equipment collection/export |
| Expedition `MainTask`, `Time` | `gamestate/mod.rs`, complete 8-word `expeditions` records, `[0]` target and `[6]` seconds | Choices, history and per-thirst statistics |
| Expedition state `Stage`, `CurrentStatus`, `CurrentHeroism` | Same file, `expeditionstate[0]` current floor, `[2]` floor stage, `[13]` heroism | Progress tracking and statistics |
| Crossroad `Left`, `Mid`, `Right` | `Expedition::update_encounters`, encounter/heroism pairs, first words of each complete pair | Current expedition choices |
| Item reward count | Current key is `expeditionreward`; Rust's comment uses 19-word item records, with the standard item type encoding | Items per thirst statistic |

## Representation and dispatch

- TS classes remain one based for the existing simulation API. Rust converts the same wire class to a zero based enum using `a - 1`. TS rune IDs remain 1..12 for the API; Rust keeps wire IDs 31..42.
- Attributes are numbers containing only the totals actually consumed by the extension. The old placeholder breakdown (`Purchased`, `Items`, `Gems`, etc.) is gone.
- Companion armor is derived from equipment, as in Rust's `Fighter::from`. Companion levels come from their own records.
- Dungeon indices are mapped before filtering gaps and special slots. Light and shadow responses update independently, preserving omitted progress. Locked/finished states become -1/-2 for the simulation API, whose enemy lookup skips both. The Rust completion rules are retained, including the generic ten-floor rule. Training camp is valid Rust data at index 30 but has no simulation API dungeon, so no position is invented for it.
- Server timestamps establish a per-server clock difference; portal day comparisons use the server's calendar and advance between responses. Local timezone/DST does not alter the ordinal.
- Empty lists remain empty. Interior empty numeric slots are errors instead of being removed and shifting offsets. Missing required words throw an Error instead of reading zero.
- Retired `ownplayersave` and `tavernspecial*` keys are ignored, matching the Rust dispatch.
- Witch scroll availability comes from `witchshop`; the obsolete `witch` key is ignored. Known scroll IDs are retained across updates for each character, matching Rust's enchantment map. The warning service requires character level 66 or higher.
- Login/Update responses are processed even when the request has no parameters.

## Reference gaps

The supplied login capture extends `events` to 12 fields, with `hv` at index 11 identifying Hellevator alongside the numeric flags at index 1. This string field is absent from the Rust snapshot. The extension preserves string fields for this response and recognizes `hv` at the observed offset, retaining Rust's minimum length and replacement semantics.

The current expedition UI reads `SideTasks` from words 1..3 of each available expedition. The Rust `AvailableExpedition` parser skips these words entirely. This used UI field is retained as legacy compatibility data; its meaning cannot be verified from the supplied Rust implementation.

Rust deliberately does not decode `expeditionrewardresources`. The extension now consumes only the presence of that message to count chest rewards. It no longer parses its resource amounts. Rust also leaves expedition item rewards out of its state; the item-record format is documented in its `expeditionreward` branch and implemented by its ordinary item parser.

## Removed data

Unused combined-save fields (registration, XP, rank, mount, action, fortress, calendar, toilet details, purchased attributes, summer tasks), resources, backpack, dummy equipment, unused guild metadata, pet collection statistics, witch cauldron/date information, item prices/gems/colors/attributes/model IDs/upgrades, and item morph/upgrade/scrapbook/rune conversion helpers have been removed.

Expedition state fields, crossroad heroism, backpacks, halftime choices and reward amounts that were only copied into unread history properties are no longer parsed or written. New history records store chest/item counts. Existing history arrays are still read for their lengths, so historical statistics work without a database migration or deletion.

## Checks

`npm run test:parsers` exercises fixed Rust-layout fixtures, parser boundaries, account isolation, partial updates, server day rollover, modern response dispatch, combat inputs and history compatibility. `node tests/event-response.test.cjs` checks the captured mixed event response, numeric flags, string codes and session updates. The Angular production build also checks templates. Other fixtures are synthetic protocol records derived from the supplied Rust code.
