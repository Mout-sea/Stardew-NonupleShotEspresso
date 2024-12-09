using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley.GameData.Buffs;
using StardewValley.GameData.Objects;

namespace NonupleShotEspresso;

public class ModEntry : Mod {
    public override void Entry(IModHelper helper) {
        helper.Events.Content.AssetRequested += OnAssetRequested;
    }

    private void OnAssetRequested(object? sender, AssetRequestedEventArgs e) {
        if (e.Name.IsEquivalentTo("Data/Objects")) {
            e.Edit(asset => {
                var dict = asset.AsDictionary<string, ObjectData>();

                var itemNonupleShotEspresso = new ObjectData {
                    Name = "Nonuple_Shot_Espresso",
                    DisplayName = "Nonuple Shot Espresso",
                    Description = "It's more potent than regular triple shot espresso!",
                    Type = "Cooking",
                    Category = StardewValley.Object.CookingCategory,
                    Price = 1350,
                    Texture = "Nonuple_Shot_Espresso",
                    Edibility = 9,
                    IsDrink = true,
                    Buffs = new List<ObjectBuffData> {
                        new ObjectBuffData {
                            Id = "Drink",
                            BuffId = null,
                            Duration = -2,
                            CustomAttributes = new BuffAttributesData {
                                Speed = 1.0F
                            }
                        }
                    }
                };
                
                dict.Data["Nonuple_Shot_Espresso"] = itemNonupleShotEspresso;

                Monitor.Log("Add Nonuple Shot Espresso!");
            });

        }

        if (e.Name.IsEquivalentTo("Nonuple_Shot_Espresso")) {
            e.LoadFromModFile<Texture2D>("assets/Nonuple_Shot_Espresso.png", AssetLoadPriority.Medium);
        }

        if (e.Name.IsEquivalentTo("Data/CookingRecipes")) {
            e.Edit(asset => {
                var dict = asset.AsDictionary<string, string>();
                dict.Data["Nonuple_Shot_Espresso"] = "253 3/1 10/Nonuple_Shot_Espresso/default/"; // TODO - bought from Gus
            });
        }
    }
}