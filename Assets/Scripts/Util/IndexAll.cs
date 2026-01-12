using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Util {
    public class IndexAll {
        // 根据名称返回显示名
        public static String nameToNameShow(String name) {
            if (name.Equals("Bedrock")) return "基岩";
            if (name.Equals("Coalblock")) return "煤炭块";
            if (name.Equals("CoalOre")) return "煤炭矿石";
            if (name.Equals("CobbleStone")) return "圆石";
            if (name.Equals("CraftingTable")) return "工作台";
            if (name.Equals("DiamondBlock")) return "钻石块";
            if (name.Equals("DiamondOre")) return "钻石矿石";
            if (name.Equals("Dirt")) return "泥土";
            if (name.Equals("DoorWoodLower")) return "木门上层";
            if (name.Equals("DoorWoodUpper")) return "木门下层";
            if (name.Equals("Glass")) return "玻璃";
            if (name.Equals("GoldBlock")) return "金块";
            if (name.Equals("GoldOre")) return "金矿石";
            if (name.Equals("GrassBlock")) return "草方块";
            if (name.Equals("IronBlock")) return "铁块";
            if (name.Equals("IronOre")) return "铁矿石";
            if (name.Equals("Ladder")) return "梯子";
            if (name.Equals("Leaves")) return "树叶";
            if (name.Equals("LogOak")) return "橡木原木";
            if (name.Equals("Plank")) return "橡木木板";
            if (name.Equals("Sand")) return "沙子";
            if (name.Equals("Stone")) return "石头";
            if (name.Equals("WaterFlow")) return "静止的水";
            if (name.Equals("WaterStill")) return "流动的水";
            if (name.Equals("WoodSword")) return "木剑";
            if (name.Equals("WoodPickaxe")) return "木镐";
            if (name.Equals("WoodAxe")) return "木斧";
            if (name.Equals("WoodShovel")) return "木锹";
            if (name.Equals("WoodHoe")) return "木锄";
            if (name.Equals("WoodHammer")) return "木锤";
            if (name.Equals("StoneSword")) return "石剑";
            if (name.Equals("StonePickaxe")) return "石镐";
            if (name.Equals("StoneAxe")) return "石斧";
            if (name.Equals("StoneShovel")) return "石锹";
            if (name.Equals("StoneHoe")) return "石锄";
            if (name.Equals("StoneHammer")) return "石锤";
            if (name.Equals("IronSword")) return "铁剑";
            if (name.Equals("IronPickaxe")) return "铁镐";
            if (name.Equals("IronAxe")) return "铁斧";
            if (name.Equals("IronShovel")) return "铁锹";
            if (name.Equals("IronHoe")) return "铁锄";
            if (name.Equals("IronHammer")) return "铁锤";
            if (name.Equals("GoldSword")) return "金剑";
            if (name.Equals("GoldPickaxe")) return "金镐";
            if (name.Equals("GoldAxe")) return "金斧";
            if (name.Equals("GoldShovel")) return "金锹";
            if (name.Equals("GoldHoe")) return "金锄";
            if (name.Equals("GoldHammer")) return "金锤";
            if (name.Equals("DiamondSword")) return "钻石剑";
            if (name.Equals("DiamondPickaxe")) return "钻石镐";
            if (name.Equals("DiamondAxe")) return "钻石斧";
            if (name.Equals("DiamondShovel")) return "钻石锹";
            if (name.Equals("DiamondHoe")) return "钻石锄";
            if (name.Equals("DiamondHammer")) return "钻石锤";
            if (name.Equals("IronHelmet")) return "铁头盔";
            if (name.Equals("IronChestplate")) return "铁胸甲";
            if (name.Equals("IronLeggings")) return "铁护腿";
            if (name.Equals("IronBoots")) return "铁靴子";
            if (name.Equals("GoldHelmet")) return "金头盔";
            if (name.Equals("GoldChestplate")) return "金胸甲";
            if (name.Equals("GoldLeggings")) return "金护腿";
            if (name.Equals("GoldBoots")) return "金靴子";
            if (name.Equals("DiamondHelmet")) return "钻石头盔";
            if (name.Equals("DiamondChestplate")) return "钻石胸甲";
            if (name.Equals("DiamondLeggings")) return "钻石护腿";
            if (name.Equals("DiamondBoots")) return "钻石靴子";
            if (name.Equals("Apple")) return "苹果";
            if (name.Equals("BucketEmpty")) return "桶";
            if (name.Equals("BucketWater")) return "水桶";
            if (name.Equals("SaplingOak")) return "橡树树苗";
            if (name.Equals("DoorWood")) return "木门";
            if (name.Equals("Coal")) return "煤炭";
            if (name.Equals("IronIngot")) return "铁锭";
            if (name.Equals("GoldIngot")) return "金锭";
            if (name.Equals("Diamond")) return "钻石";
            if (name.Equals("Stick")) return "木棍";
            if (name.Equals("Torch")) return "火把";
            if (name.Equals("DirtWall")) return "泥土墙";
            if (name.Equals("StoneWall")) return "石墙";
            if (name.Equals("SandWall")) return "沙墙";
            if (name.Equals("BedrockWall")) return "基岩墙";
            if (name.Equals("PlankWall")) return "木板墙";
            if (name.Equals("StairsOak")) return "橡木楼梯";
            if (name.Equals("StairsOak")) return "橡木楼梯";
            if (name.Equals("Bookshelf")) return "书架";
            if (name.Equals("Bricks")) return "红砖";
            if (name.Equals("ChiseledSandstone")) return "錾制沙石";
            if (name.Equals("Furnace")) return "熔炉";
            if (name.Equals("Chest")) return "箱子";
            if (name.Equals("CharCoal")) return "木炭";
            if (name.Equals("CharTorch")) return "木炭火把";
            return "null";
        }
        
        // 根据方块名返回音频类型
        public static String blockToAudioType(String blockName) {
            if (blockName.Equals("GrassBlock")) return "grass";
            if (blockName.Equals("Leaves")) return "grass";
            if (blockName.Equals("SaplingOak")) return "grass";
            if (blockName.Equals("Dirt")) return "gravel";
            if (blockName.Equals("DirtWall")) return "gravel";
            if (blockName.Equals("Sand")) return "sand";
            if (blockName.Equals("SandWall")) return "sand";
            if (blockName.Equals("Ladder")) return "ladder";
            if (blockName.Equals("Glass")) return "glass";
            if (blockName.Equals("CraftingTable")) return "wood";
            if (blockName.Equals("LogOak")) return "wood";
            if (blockName.Equals("Plank")) return "wood";
            if (blockName.Equals("DoorWood")) return "wood";
            if (blockName.Equals("DoorWoodLower")) return "wood";
            if (blockName.Equals("DoorWoodUpper")) return "wood";
            if (blockName.Equals("DoorWoodSideLower")) return "wood";
            if (blockName.Equals("DoorWoodSideUpper")) return "wood";
            if (blockName.Equals("Torch")) return "wood";
            if (blockName.Equals("CharTorch")) return "wood";
            if (blockName.Equals("StairsOak")) return "wood";
            if (blockName.Equals("StairsOakLeft")) return "wood";
            if (blockName.Equals("StairsOakRight")) return "wood";
            if (blockName.Equals("PlankWall")) return "wood";
            if (blockName.Equals("Chest")) return "wood";
            if (blockName.Equals("StoneWall")) return "stone";
            if (blockName.Equals("BedrockWall")) return "stone";
            if (blockName.Equals("Furnace")) return "stone";
            if (blockName.Equals("FurnaceOff")) return "stone";
            if (blockName.Equals("FurnaceOn")) return "stone";
            return "null";
        }
        
        // 根据方块名返回方块类型
        public static String blockToType(String blockName) {
            if (blockName.Equals("GrassBlock")) return "gravel";
            if (blockName.Equals("Dirt")) return "gravel";
            if (blockName.Equals("Sand")) return "gravel";
            if (blockName.Equals("SaplingOak")) return "instant";
            if (blockName.Equals("Stone")) return "stone";
            if (blockName.Equals("CobbleStone")) return "stone";
            if (blockName.Equals("CoalOre")) return "stone";
            if (blockName.Equals("CoalBlock")) return "stone";
            if (blockName.Equals("IronOre")) return "stone";
            if (blockName.Equals("IronBlock")) return "stone";
            if (blockName.Equals("GoldOre")) return "stone";
            if (blockName.Equals("GoldBlock")) return "stone";
            if (blockName.Equals("DiamondOre")) return "stone";
            if (blockName.Equals("DiamondBlock")) return "stone";
            if (blockName.Equals("Furnace")) return "stone";
            if (blockName.Equals("FurnaceOff")) return "stone";
            if (blockName.Equals("FurnaceOn")) return "stone";
            if (blockName.Equals("Ladder")) return "wood";
            if (blockName.Equals("CraftingTable")) return "wood";
            if (blockName.Equals("LogOak")) return "wood";
            if (blockName.Equals("Plank")) return "wood";
            if (blockName.Equals("DoorWood")) return "wood";
            if (blockName.Equals("DoorWoodLower")) return "wood";
            if (blockName.Equals("DoorWoodUpper")) return "wood";
            if (blockName.Equals("DoorWoodSideLower")) return "wood";
            if (blockName.Equals("DoorWoodSideUpper")) return "wood";
            if (blockName.Equals("StairsOak")) return "wood";
            if (blockName.Equals("StairsOakLeft")) return "wood";
            if (blockName.Equals("StairsOakRight")) return "wood";
            if (blockName.Equals("Chest")) return "wood";
            if (blockName.Equals("Torch")) return "instant";
            if (blockName.Equals("CharTorch")) return "instant";
            if (blockName.Equals("DirtWall")) return "wall";
            if (blockName.Equals("StoneWall")) return "wall";
            if (blockName.Equals("PlankWall")) return "wall";
            if (blockName.Equals("SandWall")) return "wall";
            if (blockName.Equals("BedrockWall")) return "wall";
            return "null";
        }
        
        // 根据名称返回是否是方块
        public static bool NameToIsBlock(String name) {
            if (name.Equals("Bedrock")) return true;
            if (name.Equals("CoalBlock")) return true;
            if (name.Equals("CoalOre")) return true;
            if (name.Equals("CobbleStone")) return true;
            if (name.Equals("CraftingTable")) return true;
            if (name.Equals("DiamondBlock")) return true;
            if (name.Equals("DiamondOre")) return true;
            if (name.Equals("Dirt")) return true;
            if (name.Equals("DoorWood")) return true;
            if (name.Equals("DoorWoodLower")) return true;
            if (name.Equals("DoorWoodUpper")) return true;
            if (name.Equals("DoorWoodSideLower")) return true;
            if (name.Equals("DoorWoodSideUpper")) return true;
            if (name.Equals("Glass")) return true;
            if (name.Equals("GoldBlock")) return true;
            if (name.Equals("GoldOre")) return true;
            if (name.Equals("GrassBlock")) return true;
            if (name.Equals("IronBlock")) return true;
            if (name.Equals("IronOre")) return true;
            if (name.Equals("Ladder")) return true;
            if (name.Equals("Leaves")) return true;
            if (name.Equals("LogOak")) return true;
            if (name.Equals("Plank")) return true;
            if (name.Equals("Sand")) return true;
            if (name.Equals("Stone")) return true;
            if (name.Equals("WaterFlow")) return true;
            if (name.Equals("WaterStill")) return true;
            if (name.Equals("Torch")) return true;
            if (name.Equals("CharTorch")) return true;
            if (name.Equals("StairsOak")) return true;
            if (name.Equals("StairsOakLeft")) return true;
            if (name.Equals("StairsOakRight")) return true;
            if (name.Equals("DoorWood")) return true;
            if (name.Equals("DirtWall")) return true;
            if (name.Equals("StoneWall")) return true;
            if (name.Equals("PlankWall")) return true;
            if (name.Equals("SandWall")) return true;
            if (name.Equals("BedrockWall")) return true;
            if (name.Equals("Furnace")) return true;
            if (name.Equals("FurnaceOff")) return true;
            if (name.Equals("FurnaceOn")) return true;
            if (name.Equals("Chest")) return true;
            if (name.Equals("SaplingOak")) return true;
            return false;
        }
        
        // 根据名称返回是否是消耗品
        public static bool nameToIsDurable(String name) {
            if (name.Equals("WoodSword")) return true;
            if (name.Equals("WoodPickaxe")) return true;
            if (name.Equals("WoodAxe")) return true;
            if (name.Equals("WoodShovel")) return true;
            if (name.Equals("WoodHoe")) return true;
            if (name.Equals("WoodHammer")) return true;
            if (name.Equals("StoneSword")) return true;
            if (name.Equals("StonePickaxe")) return true;
            if (name.Equals("StoneAxe")) return true;
            if (name.Equals("StoneShovel")) return true;
            if (name.Equals("StoneHoe")) return true;
            if (name.Equals("StoneHammer")) return true;
            if (name.Equals("IronSword")) return true;
            if (name.Equals("IronPickaxe")) return true;
            if (name.Equals("IronAxe")) return true;
            if (name.Equals("IronShovel")) return true;
            if (name.Equals("IronHoe")) return true;
            if (name.Equals("IronHammer")) return true;
            if (name.Equals("GoldSword")) return true;
            if (name.Equals("GoldPickaxe")) return true;
            if (name.Equals("GoldAxe")) return true;
            if (name.Equals("GoldShovel")) return true;
            if (name.Equals("GoldHoe")) return true;
            if (name.Equals("GoldHammer")) return true;
            if (name.Equals("DiamondSword")) return true;
            if (name.Equals("DiamondPickaxe")) return true;
            if (name.Equals("DiamondAxe")) return true;
            if (name.Equals("DiamondShovel")) return true;
            if (name.Equals("DiamondHoe")) return true;
            if (name.Equals("DiamondHammer")) return true;
            if (name.Equals("IronHelmet")) return true;
            if (name.Equals("IronChestplate")) return true;
            if (name.Equals("IronLeggings")) return true;
            if (name.Equals("IronBoots")) return true;
            if (name.Equals("GoldHelmet")) return true;
            if (name.Equals("GoldChestplate")) return true;
            if (name.Equals("GoldLeggings")) return true;
            if (name.Equals("GoldBoots")) return true;
            if (name.Equals("DiamondHelmet")) return true;
            if (name.Equals("DiamondChestplate")) return true;
            if (name.Equals("DiamondLeggings")) return true;
            if (name.Equals("DiamondBoots")) return true;
            return false;
        }
        
        // 根据名称返回是否是盔甲
        public static bool nameToIsArmor(String name) {
            if (name.Equals("IronHelmet")) return true;
            if (name.Equals("IronChestplate")) return true;
            if (name.Equals("IronLeggings")) return true;
            if (name.Equals("IronBoots")) return true;
            if (name.Equals("GoldHelmet")) return true;
            if (name.Equals("GoldChestplate")) return true;
            if (name.Equals("GoldLeggings")) return true;
            if (name.Equals("GoldBoots")) return true;
            if (name.Equals("DiamondHelmet")) return true;
            if (name.Equals("DiamondChestplate")) return true;
            if (name.Equals("DiamondLeggings")) return true;
            if (name.Equals("DiamondBoots")) return true;
            return false;
        }
        
        // 根据名称返回是否是工具
        public static bool nameToIsTool(String name) {
            if (name.Equals("WoodSword")) return true;
            if (name.Equals("WoodPickaxe")) return true;
            if (name.Equals("WoodAxe")) return true;
            if (name.Equals("WoodShovel")) return true;
            if (name.Equals("WoodHoe")) return true;
            if (name.Equals("WoodHammer")) return true;
            if (name.Equals("StoneSword")) return true;
            if (name.Equals("StonePickaxe")) return true;
            if (name.Equals("StoneAxe")) return true;
            if (name.Equals("StoneShovel")) return true;
            if (name.Equals("StoneHoe")) return true;
            if (name.Equals("StoneHammer")) return true;
            if (name.Equals("IronSword")) return true;
            if (name.Equals("IronPickaxe")) return true;
            if (name.Equals("IronAxe")) return true;
            if (name.Equals("IronShovel")) return true;
            if (name.Equals("IronHoe")) return true;
            if (name.Equals("IronHammer")) return true;
            if (name.Equals("GoldSword")) return true;
            if (name.Equals("GoldPickaxe")) return true;
            if (name.Equals("GoldAxe")) return true;
            if (name.Equals("GoldShovel")) return true;
            if (name.Equals("GoldHoe")) return true;
            if (name.Equals("GoldHammer")) return true;
            if (name.Equals("DiamondSword")) return true;
            if (name.Equals("DiamondPickaxe")) return true;
            if (name.Equals("DiamondAxe")) return true;
            if (name.Equals("DiamondShovel")) return true;
            if (name.Equals("DiamondHoe")) return true;
            if (name.Equals("DiamondHammer")) return true;
            return false;
        }
        
        // 根据名称返回最大堆叠数
        public static int nameToMaxAmount(String name) {
            if (name.Equals("WoodSword")) return 59;
            if (name.Equals("WoodPickaxe")) return 59;
            if (name.Equals("WoodAxe")) return 59;
            if (name.Equals("WoodShovel")) return 59;
            if (name.Equals("WoodHoe")) return 59;
            if (name.Equals("WoodHammer")) return 59;
            if (name.Equals("StoneSword")) return 131;
            if (name.Equals("StonePickaxe")) return 131;
            if (name.Equals("StoneAxe")) return 131;
            if (name.Equals("StoneShovel")) return 131;
            if (name.Equals("StoneHoe")) return 131;
            if (name.Equals("StoneHammer")) return 131;
            if (name.Equals("IronSword")) return 250;
            if (name.Equals("IronPickaxe")) return 250;
            if (name.Equals("IronAxe")) return 250;
            if (name.Equals("IronShovel")) return 250;
            if (name.Equals("IronHoe")) return 250;
            if (name.Equals("IronHammer")) return 250;
            if (name.Equals("GoldSword")) return 32;
            if (name.Equals("GoldPickaxe")) return 32;
            if (name.Equals("GoldAxe")) return 32;
            if (name.Equals("GoldShovel")) return 32;
            if (name.Equals("GoldHoe")) return 32;
            if (name.Equals("GoldHammer")) return 32;
            if (name.Equals("DiamondSword")) return 1561;
            if (name.Equals("DiamondPickaxe")) return 1561;
            if (name.Equals("DiamondAxe")) return 1561;
            if (name.Equals("DiamondShovel")) return 1561;
            if (name.Equals("DiamondHoe")) return 1561;
            if (name.Equals("DiamondHammer")) return 1561;
            if (name.Equals("IronHelmet")) return 165;
            if (name.Equals("IronChestplate")) return 240;
            if (name.Equals("IronLeggings")) return 225;
            if (name.Equals("IronBoots")) return 195;
            if (name.Equals("GoldHelmet")) return 77;
            if (name.Equals("GoldChestplate")) return 112;
            if (name.Equals("GoldLeggings")) return 105;
            if (name.Equals("GoldBoots")) return 91;
            if (name.Equals("DiamondHelmet")) return 363;
            if (name.Equals("DiamondChestplate")) return 528;
            if (name.Equals("DiamondLeggings")) return 495;
            if (name.Equals("DiamondBoots")) return 429;
            if (name.Equals("DoorWood")) return 1;
            if (name.Equals("BucketWater")) return 1;
            return 64;
        }
        
        // 根据名称返回护甲值
        public static int nameToArmorValue(String name) {
            if (name.Equals("IronHelmet")) return 2;
            if (name.Equals("IronChestplate")) return 6;
            if (name.Equals("IronLeggings")) return 5;
            if (name.Equals("IronBoots")) return 2;
            if (name.Equals("GoldHelmet")) return 2;
            if (name.Equals("GoldChestplate")) return 5;
            if (name.Equals("GoldLeggings")) return 3;
            if (name.Equals("GoldBoots")) return 1;
            if (name.Equals("DiamondHelmet")) return 3;
            if (name.Equals("DiamondChestplate")) return 8;
            if (name.Equals("DiamondLeggings")) return 6;
            if (name.Equals("DiamondBoots")) return 3;
            return 0;
        }
        
        // 根据方块名和工具返回破坏时长
        public static float nameToDestroyTime(String blockName, String toolName) {
            if (blockName.Equals("Bedrock")) return -1;
            if (blockToType(blockName).Equals("back")) {
                if (toolName.Equals("WoodHammer")) return 1.5f;
                if (toolName.Equals("StoneHammer")) return 0.75f;
                if (toolName.Equals("IronHammer")) return 0.5f;
                if (toolName.Equals("GoldHammer")) return 0.25f;
                if (toolName.Equals("DiamondHammer")) return 0.33f;
                return 3;
            }
            if (blockToType(blockName).Equals("stone")) {
                if (toolName.Equals("WoodPickaxe")) return 4;
                if (toolName.Equals("StonePickaxe")) return 1;
                if (toolName.Equals("IronPickaxe")) return 0.5f;
                if (toolName.Equals("GoldPickaxe")) return 0.25f;
                if (toolName.Equals("DiamondPickaxe")) return 0.33f;
                return 8;
            }
            if (blockToType(blockName).Equals("wood")) {
                if (toolName.Equals("WoodAxe")) return 1.5f;
                if (toolName.Equals("StoneAxe")) return 0.75f;
                if (toolName.Equals("IronAxe")) return 0.5f;
                if (toolName.Equals("GoldAxe")) return 0.25f;
                if (toolName.Equals("DiamondAxe")) return 0.33f;
                return 3;
            }
            if (blockToType(blockName).Equals("gravel")) {
                if (toolName.Equals("WoodShovel")) return 0.4f;
                if (toolName.Equals("StoneShovel")) return 0.2f;
                if (toolName.Equals("IronShovel")) return 0.13f;
                if (toolName.Equals("GoldShovel")) return 0.067f;
                if (toolName.Equals("DiamondShovel")) return 0.089f;
                return 0.8f;
            }
            if (blockToType(blockName).Equals("instant")) {
                return 0.01f;
            }
            return 1;
        }
        
        // 根据工具状态查询挖掘等级
        public static int toolStateToMineLevel(String toolState) {
            if (toolState.Equals("WoodPickaxe")) return 2;
            if (toolState.Equals("StonePickaxe")) return 3;
            if (toolState.Equals("IronPickaxe")) return 4;
            if (toolState.Equals("GoldPickaxe")) return 2;
            if (toolState.Equals("DiamondPickaxe")) return 4;
            return 1;
        }
        
        // 根据方块名查询破坏后掉落物名称
        public static String blockNameToItemName(String blockName, String toolState) {
            if (blockName.Equals("GrassBlock")) return "Dirt";
            if (blockName.Equals("Stone")) {
                if(toolStateToMineLevel(toolState) >= 2) return "CobbleStone";
                return "Air";
            }
            if (blockName.Equals("CoalOre")) {
                if(toolStateToMineLevel(toolState) >= 2) return "Coal";
                return "Air";
            }
            if (blockName.Equals("IronOre")) {
                if(toolStateToMineLevel(toolState) >= 3) return "IronOre";
                return "Air";
            }
            if (blockName.Equals("GoldOre")) {
                if(toolStateToMineLevel(toolState) >= 4) return "GoldOre";
                return "Air";
            }
            if (blockName.Equals("DiamondOre")) {
                if(toolStateToMineLevel(toolState) >= 4) return "Diamond";
                return "Air";
            }
            if (blockName.Equals("DoorWoodLower")) return "DoorWood";
            if (blockName.Equals("DoorWoodUpper")) return "DoorWood";
            if (blockName.Equals("DoorWoodSideLower")) return "DoorWood";
            if (blockName.Equals("DoorWoodSideUpper")) return "DoorWood";
            if (blockName.Equals("FurnaceOff")) return "Furnace";
            if (blockName.Equals("FurnaceOn")) return "Furnace";
            if (blockName.Equals("Leaves")) return "Air";
            if (blockName.Equals("Glass")) return "Air";
            if (blockName.Equals("WaterFlow")) return "Air";
            if (blockName.Equals("WaterStill")) return "Air";
            if (blockName.Equals("StairsOak")) return "StairsOak";
            if (blockName.Equals("StairsOakLeft")) return "StairsOak";
            if (blockName.Equals("StairsOakRight")) return "StairsOak";
            return blockName;
        }
        
        // 根据方块名查询是否是无碰撞方块
        public static bool BlockNameToUntouchable(String blockName) {
            if (blockName.Equals("Torch")) return true;
            if (blockName.Equals("CharTorch")) return true;
            if (blockName.Equals("Ladder")) return true;
            if (blockName.Equals("CraftingTable")) return true;
            if (blockName.Equals("Furnace")) return true;
            if (blockName.Equals("Chest")) return true;
            if (blockName.Equals("SaplingOak")) return true;
            return false;
        }
        
        // 根据方块名查询是否是发光方块
        public static bool BlockNameToIsLight(String blockName) {
            if (blockName.Equals("Torch")) return true;
            if (blockName.Equals("CharTorch")) return true;
            return false;
        }
        
        // 根据方块名查询是否是需要依托墙壁或地面方块
        public static bool BlockNameToIsAttachable(String blockName) {
            if (blockName.Equals("Torch")) return true;
            if (blockName.Equals("CharTorch")) return true;
            if (blockName.Equals("Ladder")) return true;
            return false;
        }
        
        // 根据方块名查询是否是有朝向方块
        public static bool BlockNameToHasDirection(String blockName) {
            if (blockName.Equals("StairsOak")) return true;
            if (blockName.Equals("StairsOakLeft")) return true;
            if (blockName.Equals("StairsOakRight")) return true;
            return false;
        }
        
        // 根据名称返回作为熔炉材料产物
        public static String nameToBurnProduct(String name) {
            if (name.Equals("LogOak")) return "CharCoal";
            if (name.Equals("CobbleStone")) return "Stone";
            if (name.Equals("DiamondOre")) return "Diamond";
            if (name.Equals("GoldOre")) return "GoldIngot";
            if (name.Equals("IronOre")) return "IronIngot";
            if (name.Equals("CoalOre")) return "Coal";
            if (name.Equals("Sand")) return "Glass";
            return "null";
        }
        
        // 根据名称返回作为熔炉燃料可提供燃烧时间
        public static float nameToBurnTime(String name) {
            if (name.Equals("Coalblock")) return 800f;
            if (name.Equals("CraftingTable")) return 150f;
            if (name.Equals("Ladder")) return 28.125f;
            if (name.Equals("LogOak")) return 150f;
            if (name.Equals("Plank")) return 37.5f;
            if (name.Equals("WoodSword")) return 100f;
            if (name.Equals("WoodPickaxe")) return 100f;
            if (name.Equals("WoodAxe")) return 100f;
            if (name.Equals("WoodShovel")) return 100f;
            if (name.Equals("WoodHoe")) return 100f;
            if (name.Equals("WoodHammer")) return 100f;
            if (name.Equals("SaplingOak")) return 50f;
            if (name.Equals("DoorWood")) return 100f;
            if (name.Equals("Coal")) return 80f;
            if (name.Equals("CharCoal")) return 80f;
            if (name.Equals("Stick")) return 50f;
            if (name.Equals("PlankWall")) return 37.5f;
            if (name.Equals("StairsOak")) return 84.375f;
            if (name.Equals("Chest")) return 150f;
            return 0;
        }
        
        // 根据名称返回食物回复饱食度
        public static int nameToFoodValue(String name) {
            if (name.Equals("Apple")) return 4;
            return 0;
        }
        
        // 根据名称返回是否是透明方块（非物理意义上透明）
        public static bool NameToIsTransparent(String name)
        {
            if (name.Equals("Air")) return true;
            if (name.Equals("CraftingTable")) return true;
            if (name.Equals("Furnace")) return true;
            if (name.Equals("FurnaceOff")) return true;
            if (name.Equals("DoorWoodLower")) return true;
            if (name.Equals("DoorWoodUpper")) return true;
            if (name.Equals("Glass")) return true;
            if (name.Equals("Ladder")) return true;
            if (name.Equals("Leaves")) return true;
            if (name.Equals("WaterFlow")) return true;
            if (name.Equals("WaterStill")) return true;
            if (name.Equals("SaplingOak")) return true;
            if (name.Equals("DoorWood")) return true;
            if (name.Equals("Torch")) return true;
            if (name.Equals("CharTorch")) return true;
            if (name.Equals("DirtWall")) return true;
            if (name.Equals("StoneWall")) return true;
            if (name.Equals("SandWall")) return true;
            if (name.Equals("BedrockWall")) return true;
            if (name.Equals("PlankWall")) return true;
            if (name.Equals("StairsOak")) return true;
            if (name.Equals("StairsOak")) return true;
            if (name.Equals("Furnace")) return true;
            if (name.Equals("Chest")) return true;
            if (name.Equals("SaplingOak")) return true;
            return false;
        }
        
        // 根据时间数字转换24h制时间
        public static String numberToTime(int number) {
            if (number < 0 || number > 120000) {
                return "Invalid number";
            }
            int hours = number / 5000;  // 计算小时数
            int minutes = (int) ((number % 5000) / 83.33);  // 计算分钟数，将0-10000映射到0-60的范围
            //int seconds = (int) (((number % 5000) % 83) / 1.39);  // 计算秒数，将0-100映射到0-60的范围
            return hours + ":" + minutes;
        }
        
        // 获取除自己外的下一级孩子
        public static T[] GetComponentsInRealChildren<T>(GameObject go, bool includeInactive = false) where T : Component
        {
            List<T> TList = go.GetComponentsInChildren<T>(includeInactive).ToList(); 
            List<T> TListReal = new List<T>();
            for (int i = 0; i < TList.Count; i++)
            {
                if (TList[i].transform.parent == go.transform)
                {
                    TListReal.Add(TList[i]);
                }
            }
            return TListReal.ToArray();
        }
    }
}