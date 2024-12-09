using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley.GameData.Buffs;
using StardewValley.GameData.Objects;
using StardewValley.GameData.Shops;

namespace NonupleShotEspresso;

public class ModEntry : Mod {
    public override void Entry(IModHelper helper) {
        I18n.Init(helper.Translation);

        IModEvents events = helper.Events;
        events.Content.AssetRequested += OnAssetRequested;
        events.Content.LocaleChanged += OnLocaleChanged;

        if (!helper.Translation.GetTranslations().Any())
            this.Monitor.Log("The translation files in this mod's i18n folder seem to be missing. The mod will still work, but you'll see 'missing translation' messages. Try reinstalling the mod to fix this.", LogLevel.Warn);
    }

    private void OnAssetRequested(object? sender, AssetRequestedEventArgs e) {
        if (e.Name.IsEquivalentTo("Data/Objects")) {
            e.Edit(asset => {
                var dict = asset.AsDictionary<string, ObjectData>();

                var itemNonupleShotEspresso = new ObjectData {
                    Name = "Nonuple_Shot_Espresso",
                    DisplayName = I18n.Name_NonupleShotEspresso(),
                    Description = I18n.Description_NonupleShotEspresso(),
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
                dict.Data["Nonuple_Shot_Espresso"] = "253 3/1 10/Nonuple_Shot_Espresso/";
            });
        }

        if (e.Name.IsEquivalentTo("Data/Shops")) {
            e.Edit(asset => {
                var dict = asset.AsDictionary<string, ShopData>();
                dict.Data["QiGemShop"].Items.Add(new ShopItemData {
                    Id = "Nonuple_Shot_Espresso (Recipe)",
                    ItemId = "Nonuple_Shot_Espresso",
                    IsRecipe = true,
                    TradeItemId = "(O)858",
                    TradeItemAmount = 20
                });
            });
        }
    }

    private void OnLocaleChanged(object? sender, LocaleChangedEventArgs e) {
        this.Helper.GameContent.InvalidateCache("Data/Objects");
    }
}