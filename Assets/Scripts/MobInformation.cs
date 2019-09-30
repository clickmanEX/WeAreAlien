using UnityEngine;
using System.Collections;

public class MobInformation
{
    public class MobParamator
    {
        public string tagName;
        public string instructionsText;
        public string successCaptuteText;
        public int basePoint;
        public int bonusPoint;
        public bool isDamageObj;
    }

    //ScriptableObjectで管理すべきかどうか検討中
    public readonly static MobParamator[] Bonus_MobParamators = new MobParamator[]
    {
        new MobParamator ()
        {
            tagName = "Army",
            instructionsText = "地球ノ軍事力ヲ見ニハ兵器ヲ調ベルノガ一番ダ。地球ノ兵器ヲ調達セヨ！",
            successCaptuteText = "鉄ノ塊ヲ高速デ打チ出ス兵器トハオモシロイ。資源ガ豊富ナ地球ナラデハノ兵器ダナ。",
            basePoint = 1000,
            bonusPoint = 2000,
            isDamageObj = false
        },
        new MobParamator ()
        {
            tagName = "Scientist",
            instructionsText = "地球人ノ科学力ヲ見タイ。優秀ナ人物ガイレバ連レテクルノダ。",
            successCaptuteText = "Scientist Bonus",
            basePoint = 1000,
            bonusPoint = 2000,
            isDamageObj = false
        },
        new MobParamator ()
        {
            tagName = "Chef",
            instructionsText = "宇宙食ハ飽キタナァ。美味シイモノヲ食ベタイ・・・",
            successCaptuteText = "Chef Bonus",
            basePoint = 1000,
            bonusPoint = 2000,
            isDamageObj = false
        },
        new MobParamator ()
        {
            tagName = "Cat",
            instructionsText = "故郷デ待ッテイル子供ノタメニオ土産ガ欲シイ。何カメボシイモノハナイカ？",
            successCaptuteText = "Cat Bonus",
            basePoint = 1000,
            bonusPoint = 2000,
            isDamageObj = false
        },
        new MobParamator ()
        {
            tagName = "Dog",
            instructionsText = "故郷デ待ッテイル子供ノタメニオ土産ガ欲シイ。何カメボシイモノハナイカ？",
            successCaptuteText = "Dog Bonus",
            basePoint = 1000,
            bonusPoint = 2000,
            isDamageObj = false
        },
        new MobParamator ()
        {
            tagName = "Alien",
            instructionsText = "作戦中ノ戦闘機ガ墜落シタヨウダ！同胞ヲ見ツケ次第大至急回収セヨ！",
            successCaptuteText = "Alien Bonus",
            basePoint = 1000,
            bonusPoint = 2000,
            isDamageObj = false
        },
        new MobParamator ()
        {
            tagName = "Ambulance",
            instructionsText = "我ガ星の遺伝子強化ヲ進メルニハ強力ナ遺伝子ガ必要ダ。サンプルヲ採取セヨ！",
            successCaptuteText = "医療品ガ沢山積ンダモノガ近クヲ通ッテ助カッタ。アリガタクイタダクトシヨウ。",
            basePoint =  -10000,
            bonusPoint = 5000,
            isDamageObj = true
        },
        new MobParamator ()
        {
            tagName = "Bear",
            instructionsText = "作戦中ノ戦闘機ガ墜落シタヨウダ！同胞ヲ見ツケ次第大至急回収セヨ！",
            successCaptuteText = "Bear Bonus",
            basePoint =  -10000,
            bonusPoint = 5000,
            isDamageObj = true
        },
    };

    public readonly static MobParamator[] No_Bonus_MobParamators = new MobParamator[]
    {
        new MobParamator ()
        {
            tagName = "Human",
            instructionsText = "",
            successCaptuteText = "",
            basePoint = 1000,
            bonusPoint = 0,
            isDamageObj = false
        },
        new MobParamator ()
        {
            tagName = "Car",
            instructionsText = "",
            successCaptuteText = "",
            basePoint = -10000,
            bonusPoint = 0,
            isDamageObj = true
        }
    };

    public static MobParamator SelectMobParamator(string tagName)
    {
        MobParamator mobParam = null;
        for(int i = 0; i < Bonus_MobParamators.Length; i++)
        {
            if(string.Equals(tagName, Bonus_MobParamators[i].tagName))
            {
                mobParam = Bonus_MobParamators[i];
                return mobParam;
            }
        }

        for (int i = 0; i < No_Bonus_MobParamators.Length; i++)
        {
            if (string.Equals(tagName, No_Bonus_MobParamators[i].tagName))
            {
                mobParam = No_Bonus_MobParamators[i];
                break;
            }
        }
        return mobParam;
    }
}
