using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class ABaseCharacter : MonoBehaviour
{
    [SerializeField]
    public int currentHealth = 700;
    public int LifeStealRatio = 0;
    public float criticRatio = 0f;
    public int Armor = 10;
    public int MovementSpeed = 10;
    public int AttackPower = 100;
    public int AttackSpeed = 100;
    public float Level = 1;
    public int Experiance = 1;
    public float AttackBuffMultiplier = 0f;
    public float AttackDebuffMultiplier = 0f;
    public float DefenseBuffMultiplier = 0f;
    public float DefenseDebuffMultiplier = 0f;
    public bool isCritic=false;

    public float Damage(float characterAttackDamage, float baseAbilityDamage, float abilityDamageRatio, float highestBuffMultiplier = 0.0f, float highestDebuffMultiplier = 0.0f, float abilityDamageReduceRatio = 0.0f, float damageReduceOrPlus=0.0f)
    {
        return (characterAttackDamage + (baseAbilityDamage * (1 + (abilityDamageRatio / 100) * (1 - (abilityDamageReduceRatio / 100))))) * (1 + highestBuffMultiplier / 100) * (1 - highestDebuffMultiplier / 100) - damageReduceOrPlus + Random.Range(0,5.0f);
    }
    public float Damage(float characterAttackDamage, float abilityDamageRatio, float highestBuffMultiplier = 0.0f, float highestDebuffMultiplier = 0.0f, float abilityDamageReduceRatio = 0.0f, float damageReduceOrPlus = 0.0f)
    {
        return (characterAttackDamage * (1 + (abilityDamageRatio / 100) * (1 - (abilityDamageReduceRatio / 100)))) * (1 + highestBuffMultiplier / 100) * (1 - highestDebuffMultiplier / 100) - damageReduceOrPlus + Random.Range(0.0f, 5.0f);
    }
    public float Damage(float characterAttackDamage, float highestBuffMultiplier = 0.0f, float highestDebuffMultiplier=0.0f, float damageReduceOrPlus = 0.0f)
    {
        return characterAttackDamage * (1 + highestBuffMultiplier / 100) * (1 - highestDebuffMultiplier / 100) - damageReduceOrPlus + Random.Range(0.0f, 5.0f);
    }

    /// <summary>
    /// Kritik hasarýn hesaplanmasý. Temel hasarý en az 2 kat arttýrýr. Belli bir olasýlýk ile gerçekleþebilir, eðer gerçekleþmezse temel hasar miktarýný döndürür
    /// </summary>
    /// <param name="calculatedDamage">Temel hasar miktarý</param>
    /// <param name="criticRatio">Kritik hasar ihtimali. 0 ile 100 arasýnda bir deðer olmalýdýr. Yüksek olmasý hem kritik olma ihtimalini arttýrýr, hem de hasar miktarýný artýrýr</param>
    /// <returns>Ýhtimal gerçekleþirse kritik hasarla birlikte, gerçekleþmezse temel hasar miktarýný float cinsinden döndürür</returns>
    public float DamageWithCritic(float calculatedDamage, float criticRatio)
    {
        float criticScala = UnityEngine.Random.Range(0.0f, 100.0f);
        if (criticScala < criticRatio)
        {
            isCritic= true;
            return calculatedDamage *= (2 + (criticRatio / 200) + criticScala/100);
        }
        else
        {
            isCritic= false;
            return calculatedDamage;
        }

    }
    /// <summary>
    /// Zýrh ile hasarýn % kaçýndan korunulacaðý hesaplanýr
    /// </summary>
    /// <param name="enemyArmor">Karþý tarafýn zýrhý, hasardan korunmayý saðlar. Ne kadar çok o kadar az hasar ama belli bir zýrhtan sonra hasar azaltma miktarý azalýr. Örneðin 100 zýrhta hasar yarýya düþüyorsa 200'de tamamen azalmaz</param>
    /// <param name="maxArmorHalf">Zýrhýn maksimum deðerinin yarýsýdýr. Bu deðer ne kadar yüksek olursa zýrhýn etki o kadar az olacak, bir baþka deðiþle o kadar fazla hasar ortaya çýkacaktýr. 
    /// Mesela 100 olarak belirlenirse zýrh deðeri 100 olduðunda hasar yarý yarýya azalýr, 200 olduðunda hasarýn yarý yarýya azalmasý için zýrhýnda 200 olmasý gerekir.</param>
    /// <param name="defenseHighestBuffMultiplier">Zýrh miktarýný oransal olarak arttýran bir etkidir. 0 ile 100 arasýnda bir deðer olmalýdýr. Varsayýlan olarak 0 belirlenmiþtir</param>
    /// <param name="defenseHighestDebuffMultiplier">Zýrh miktarýný oransal olarak azaltan bir etkidir. 0 ile 100 arasýnda bir deðer olmalýdýr. Varsayýlan olarak 0 belirlenmiþtir</param>
    /// <param name="defenseReduceOrPlus">Zýrh miktarýný doðrudan azaltan veya arttýran bir etkidir.</param>
    /// <returns> Normal koþullarda 0 ile 1 arasýnda bir deðer döndürür</returns>
    public float Defense(float armor, float maxArmorHalf = 500f,float highestBuffMultiplier = 0.0f, float highestDebuffMultiplier=0.0f, float defenseReduceOrPlus = 0.0f)
    {
        /*
        float def = (100 + characterLevel) / ((100 + characterLevel) + (100 + enemyLevel) * (1 + enemyArmor / 100));
        return def;
        */
        defenseReduceOrPlus = Mathf.Clamp(defenseReduceOrPlus, -250.0f, 250.0f);
        highestDebuffMultiplier = Mathf.Clamp(highestDebuffMultiplier, 0, 100);
        return ((maxArmorHalf - armor/10) / (armor * (1 + highestBuffMultiplier / 100)*(1 - highestDebuffMultiplier / 100) - defenseReduceOrPlus + maxArmorHalf - armor/10));
    }

    /// <summary>
    /// Karakterler arasýndaki seviye farký deðerlendirilir
    /// </summary>
    /// <param name="characterLevel">Karakterin seviyesidir. 0 ile 100 arasýnda bir deðer olmasý beklenir</param>
    /// <param name="enemyLevel">Karþý tarafýn seviyesidir. 0 ile 100 arasýnda bir deðer olmasý beklenir</param>
    /// <returns> Normal koþullarda 1 ile 2 arasýnda bir deðer döndürmesi beklenir</returns>
    public float LevelDifference(float characterLevel, float enemyLevel)
    {
        return (1 + (characterLevel / 200) - (enemyLevel / 200));
    }


    /// <summary>
    /// Yetenek hasarý ile birlikte, zýrhýn koruma yüzdesi, kritik vurma þansý ve aradaki seviye farký dikkate alýnarak verilecek hasarý hesaplar. 
    /// Yetenek hasarý temel hasardan üstüne ekstra olarak eklenir
    /// Karþý tarafýn zýrh deðeri alýnarak hasar bir miktar azaltýlýr
    /// Kritik ile birlikte hasarý en az 2 katýna çýkrma ihtimali vardýr
    /// Seviye farkýna göre hasar azalabilir veya artabilir
    /// Ýlk önce karakterin saldýrý gücü ve yetenek hasarý hesaplanýr, ardýndan zýrh korumasý ile bu hasarýn bir kýsmý azalýr, 
    /// sonra kritik þansý devreye girer ve karakterler arasýndaki seviye farký deðerlendirilir. En sonunda bütün bu hesaplamardan baðýmsýz gerçek hasar eklenir veya çýkartýlýr
    /// </summary>
    /// <param name="characterAttackDamage"> Karakterin eþyalarla birlikte sahip olduðu saldýrý gücüdür, pozitif sayýlar olmasý beklenir. Ne kadar büyük o kadar fazla hasar</param>
    /// <param name="baseAbilityDamage"> Karakter gücünden baðýmsýz ekstra temel yetenek hasarý, pozitif sayýlar olmasý beklenir </param>
    /// <param name="abilityDamageRatio">Temel yetenek hasarýna yüzdelik ekstra güç ekler. Genellikle 0 ile 100 arasýnda bir deðer olmasý beklenir, 100'den fazla olabilir ama 0'dan az olmamalý. Örneðin temel yetenek hasarý 100, yetenek hasar oraný %10 toplam hasar 110</param>
    /// <param name="enemyArmor">Karþý tarafýn zýrhý, hasardan korunmayý saðlar. Ne kadar çok o kadar az hasar ama belli bir zýrhtan sonra hasar azaltma miktarý azalýr. Örneðin 100 zýrhta hasar yarýya düþüyorsa 200'de tamamen azalmaz</param>
    /// <param name="criticRatio">Kritik hasar ihtimali. 0 ile 100 arasýnda bir deðer olmalýdýr. Yüksek olmasý hem kritik olma ihtimalini arttýrýr, hem de hasar miktarýný artýrýr</param>
    /// <param name="characterLevel"> Saldýrý yapanýn seviyesi. Karþý tarafýn seviyesinden yüksekse daha fazla hasar düþükse daha az hasar ortaya çýkar</param>
    /// <param name="enemyLevel">Saldýrý hedefinin seviyesi. Saldýrý yapanýn seviyesinden yüksekse daha az hasar düþükse daha fazla hasar ortaya çýkar</param>
    /// <param name="attackHighestBuffMultiplier">Hasar miktrýný oransal olarak artýran bir buff etkisidir. 0 ile 100 arasýnda bir deðer olmalýdýr. Genellikle 0 ile 100 arasýnda bir deðer olmasý beklenir, 100'den fazla olabilir ama 0'dan az olmamalýdýr. Karþý tarafýn zýrhýndan etkilenir. Varsayýlan olarak 0 belirlenmiþtir</param>
    /// <param name="attackHighestDebuffMultiplier">Hasar miktarýný oransal olarak azaltan bir debuff etkisidir. Ýlk bölümde hasarýn hesaplanmasýnda etkilidir. 0 ile 100 arasýnda bir deðer olmalýdýr. Varsayýlan olarak 0 belirlenmiþtir</param>
    /// <param name="abilityDamageReduceRatio">Temel yetenek hasarýný oransal olarak azaltan bir debuff etkisidir. Ýlk bölümde hasarýn hesaplanmasýnda etkilidir. 0 ile 100 arasýnda bir deðer olmalýdýr. Varsayýlan olarak 0 belirlenmiþtir</param>
    /// <param name="damageReduceOrPlus">Hasar miktarýný doðrudan azaltan veya arttýran bir etkisidir. Ýlk bölümde hasarýn hesaplanmasýnda etkilidir. Pozitif sayýlar hasarý arttýrýr, negatif sayýlar hasarý azaltýr</param>
    /// <param name="defenseHighestBuffMultiplier">Zýrh miktarýný oransal olarak arttýran bir etkidir. 0 ile 100 arasýnda bir deðer olmalýdýr. Varsayýlan olarak 0 belirlenmiþtir</param>
    /// <param name="defenseHighestDebuffMultiplier">Zýrh miktarýný oransal olarak azaltan bir etkidir. 0 ile 100 arasýnda bir deðer olmalýdýr. Varsayýlan olarak 0 belirlenmiþtir</param>
    /// <param name="maxArmorHalf">Zýrhýn maksimum deðerinin yarýsýdýr. Bu deðer ne kadar yüksek olursa zýrhýn etki o kadar az olacak, bir baþka deðiþle o kadar fazla hasar ortaya çýkacaktýr. 
    /// Mesela 100 olarak belirlenirse zýrh deðeri 100 olduðunda hasar yarý yarýya azalýr, 200 olduðunda hasarýn yarý yarýya azalmasý için zýrhýnda 200 olmasý gerekir.</param>
    /// <param name="defenseReduceOrPlus">Zýrh miktarýný doðrudan azaltan veya arttýran bir etkidir.</param>
    /// <param name="trueDamagePlusOrReduce">Bütün hesaplamardan baðýmsýz bir deðerdir, gerçek hasar olarak eklenir veya çýkartýlýr</param>
    /// <returns>float tipinde sayý döndürür, bu sayý hesaplanmýþ hasar miktarýdýr</returns>
    public float CalculateDamage(float characterAttackDamage, float baseAbilityDamage, float abilityDamageRatio, 
                                float enemyArmor, float criticRatio, float characterLevel, float enemyLevel,
                                float attackHighestBuffMultiplier = 0.0f, float attackHighestDebuffMultiplier=0.0f, 
                                float abilityDamageReduceRatio = 0.0f, float damageReduceOrPlus = 0.0f, 
                                float defenseHighestBuffMultiplier = 0.0f, float defenseHighestDebuffMultiplier = 0.0f,
                                float maxArmorHalf = 500f, float defenseReduceOrPlus = 0.0f, int trueDamagePlusOrReduce = 0) 
    {
        float calculatedDamage = Damage(characterAttackDamage, baseAbilityDamage, 
                                        abilityDamageRatio, attackHighestBuffMultiplier, 
                                        attackHighestDebuffMultiplier, 
                                        abilityDamageReduceRatio, damageReduceOrPlus) * Defense(enemyArmor, maxArmorHalf, defenseHighestBuffMultiplier, 
                                                                                                defenseHighestDebuffMultiplier, defenseReduceOrPlus);
        if (criticRatio > 0)
        {
            return DamageWithCritic(calculatedDamage, criticRatio) * LevelDifference(characterLevel, enemyLevel) + trueDamagePlusOrReduce;
        }
        return calculatedDamage * LevelDifference(characterLevel, enemyLevel) + trueDamagePlusOrReduce;
    }


    /// <summary>
    /// Yetenek olmaksýzýn karakterin saldýrý gücüyle verilen temel saldýrý hasarýný hesaplar
    /// Karþý tarafýn zýrh deðeri alýnarak hasar bir miktar azaltýlýr
    /// Kritik ile birlikte hasarý en az 2 katýna çýkrma ihtimali vardýr
    /// Seviye farkýna göre hasar azalabilir veya artabilir
    /// Ýlk önce karakterin saldýrý gücü ile bir hasar hesaplanýr, ardýndan zýrh korumasý ile bu hasarýn bir kýsmý azalýr, 
    /// sonra kritik þansý devreye girer ve karakterler arasýndaki seviye farký deðerlendirilir. En sonunda bütün bu hesaplamardan baðýmsýz gerçek hasar eklenir veya çýkartýlýr
    /// </summary>
    /// <param name="characterAttackDamage"> Karakterin eþyalarla birlikte sahip olduðu saldýrý gücüdür, pozitif sayýlar olmasý beklenir. Ne kadar büyük o kadar fazla hasar</param>
    /// <param name="enemyArmor">Karþý tarafýn zýrhý, hasardan korunmayý saðlar. Ne kadar çok o kadar az hasar ama belli bir zýrhtan sonra hasar azaltma miktarý azalýr. Örneðin 100 zýrhta hasar yarýya düþüyorsa 200'de tamamen azalmaz</param>
    /// <param name="criticRatio">Kritik hasar ihtimali. 0 ile 100 arasýnda bir deðer olmalýdýr. Yüksek olmasý hem kritik olma ihtimalini arttýrýr, hem de hasar miktarýný artýrýr</param>
    /// <param name="characterLevel"> Saldýrý yapanýn seviyesi. Karþý tarafýn seviyesinden yüksekse daha fazla hasar düþükse daha az hasar ortaya çýkar</param>
    /// <param name="enemyLevel">Saldýrý hedefinin seviyesi. Saldýrý yapanýn seviyesinden yüksekse daha az hasar düþükse daha fazla hasar ortaya çýkar</param>
    /// <param name="attackHighestBuffMultiplier">Hasar miktrýný oransal olarak artýran bir buff etkisidir. 0 ile 100 arasýnda bir deðer olmalýdýr. Genellikle 0 ile 100 arasýnda bir deðer olmasý beklenir, 100'den fazla olabilir ama 0'dan az olmamalýdýr. Karþý tarafýn zýrhýndan etkilenir. Varsayýlan olarak 0 belirlenmiþtir</param>
    /// <param name="attackHighestDebuffMultiplier">Hasar miktarýný oransal olarak azaltan bir debuff etkisidir. Ýlk bölümde hasarýn hesaplanmasýnda etkilidir. 0 ile 100 arasýnda bir deðer olmalýdýr. Varsayýlan olarak 0 belirlenmiþtir</param>
    /// <param name="abilityDamageReduceRatio">Temel yetenek hasarýný oransal olarak azaltan bir debuff etkisidir. Ýlk bölümde hasarýn hesaplanmasýnda etkilidir. 0 ile 100 arasýnda bir deðer olmalýdýr. Varsayýlan olarak 0 belirlenmiþtir</param>
    /// <param name="damageReduceOrPlus">Hasar miktarýný doðrudan azaltan veya arttýran bir etkisidir. Ýlk bölümde hasarýn hesaplanmasýnda etkilidir. Pozitif sayýlar hasarý arttýrýr, negatif sayýlar hasarý azaltýr</param>
    /// <param name="defenseHighestBuffMultiplier">Zýrh miktarýný oransal olarak arttýran bir etkidir. 0 ile 100 arasýnda bir deðer olmalýdýr. Varsayýlan olarak 0 belirlenmiþtir</param>
    /// <param name="defenseHighestDebuffMultiplier">Zýrh miktarýný oransal olarak azaltan bir etkidir. 0 ile 100 arasýnda bir deðer olmalýdýr. Varsayýlan olarak 0 belirlenmiþtir</param>
    /// <param name="maxArmorHalf">Zýrhýn maksimum deðerinin yarýsýdýr. Bu deðer ne kadar yüksek olursa zýrhýn etki o kadar az olacak, bir baþka deðiþle o kadar fazla hasar ortaya çýkacaktýr. 
    /// Mesela 100 olarak belirlenirse zýrh deðeri 100 olduðunda hasar yarý yarýya azalýr, 200 olduðunda hasarýn yarý yarýya azalmasý için zýrhýnda 200 olmasý gerekir.</param>
    /// <param name="defenseReduceOrPlus">Zýrh miktarýný doðrudan azaltan veya arttýran bir etkidir.</param>
    /// <param name="trueDamagePlusOrReduce">Bütün hesaplamardan baðýmsýz bir deðerdir, gerçek hasar olarak eklenir veya çýkartýlýr</param>
    /// <returns>float tipinde sayý döndürür, bu sayý hesaplanmýþ hasar miktarýdýr</returns>
    public float CalculateDamage(float characterAttackDamage, float enemyArmor, float criticRatio, 
                                float characterLevel, float enemyLevel, float attackHighestBuffMultiplier = 0.0f, float attackHighestDebuffMultiplier = 0.0f,
                                float damageReduceOrPlus = 0.0f, float defenseHighestBuffMultiplier = 0.0f, float defenseHighestDebuffMultiplier =0.0f,
                                float maxArmorHalf = 500f, float defenseReduceOrPlus = 0.0f, int trueDamagePlusOrReduce = 0)
    {
        float calculatedDamage = Damage(characterAttackDamage, attackHighestBuffMultiplier, 
                                        attackHighestDebuffMultiplier,damageReduceOrPlus) * Defense(enemyArmor, maxArmorHalf, defenseHighestBuffMultiplier, 
                                                                                                    defenseHighestDebuffMultiplier,defenseReduceOrPlus);
        if (criticRatio > 0)
        {
            return DamageWithCritic(calculatedDamage, criticRatio) * LevelDifference(characterLevel, enemyLevel) + trueDamagePlusOrReduce;
        }
        return calculatedDamage * LevelDifference(characterLevel, enemyLevel) + trueDamagePlusOrReduce;
    }



    /// <summary>
    /// Yetenek hasarý ile birlikte, zýrhýn koruma yüzdesi, kritik vurma þansý ve aradaki seviye farký dikkate alýnarak verilecek hasarý hesaplar. 
    /// Yetenek hasarý temel saldýrý gücü baz alýnarak hesaplanýr
    /// Karþý tarafýn zýrh deðeri alýnarak hasar bir miktar azaltýlýr
    /// Kritik ile birlikte hasarý en az 2 katýna çýkrma ihtimali vardýr
    /// Seviye farkýna göre hasar azalabilir veya artabilir
    /// Ýlk önce karakterin saldýrý gücü ve yetenek oraný ile hesaplanýr, ardýndan zýrh korumasý ile bu hasarýn bir kýsmý azalýr, 
    /// sonra kritik þansý devreye girer ve karakterler arasýndaki seviye farký deðerlendirilir. En sonunda bütün bu hesaplamardan baðýmsýz gerçek hasar eklenir veya çýkartýlýr
    /// </summary>
    /// <param name="characterAttackDamage"> Karakterin eþyalarla birlikte sahip olduðu saldýrý gücüdür, pozitif sayýlar olmasý beklenir. Ne kadar büyük o kadar fazla hasar</param>
    /// <param name="abilityDamageRatio">Temel yetenek hasarýna yüzdelik ekstra güç ekler. Genellikle 0 ile 100 arasýnda bir deðer olmasý beklenir, 100'den fazla olabilir ama 0'dan az olmamalý. Örneðin temel yetenek hasarý 100, yetenek hasar oraný %10 toplam hasar 110</param>
    /// <param name="enemyArmor">Karþý tarafýn zýrhý, hasardan korunmayý saðlar. Ne kadar çok o kadar az hasar ama belli bir zýrhtan sonra hasar azaltma miktarý azalýr. Örneðin 100 zýrhta hasar yarýya düþüyorsa 200'de tamamen azalmaz</param>
    /// <param name="criticRatio">Kritik hasar ihtimali. 0 ile 100 arasýnda bir deðer olmalýdýr. Yüksek olmasý hem kritik olma ihtimalini arttýrýr, hem de hasar miktarýný artýrýr</param>
    /// <param name="characterLevel"> Saldýrý yapanýn seviyesi. Karþý tarafýn seviyesinden yüksekse daha fazla hasar düþükse daha az hasar ortaya çýkar</param>
    /// <param name="enemyLevel">Saldýrý hedefinin seviyesi. Saldýrý yapanýn seviyesinden yüksekse daha az hasar düþükse daha fazla hasar ortaya çýkar</param>
    /// <param name="attackHighestBuffMultiplier">Hasar miktrýný oransal olarak artýran bir buff etkisidir. 0 ile 100 arasýnda bir deðer olmalýdýr. Genellikle 0 ile 100 arasýnda bir deðer olmasý beklenir, 100'den fazla olabilir ama 0'dan az olmamalýdýr. Karþý tarafýn zýrhýndan etkilenir. Varsayýlan olarak 0 belirlenmiþtir</param>
    /// <param name="attackHighestDebuffMultiplier">Hasar miktarýný oransal olarak azaltan bir debuff etkisidir. Ýlk bölümde hasarýn hesaplanmasýnda etkilidir. 0 ile 100 arasýnda bir deðer olmalýdýr. Varsayýlan olarak 0 belirlenmiþtir</param>
    /// <param name="abilityDamageReduceRatio">Temel yetenek hasarýný oransal olarak azaltan bir debuff etkisidir. Ýlk bölümde hasarýn hesaplanmasýnda etkilidir. 0 ile 100 arasýnda bir deðer olmalýdýr. Varsayýlan olarak 0 belirlenmiþtir</param>
    /// <param name="damageReduceOrPlus">Hasar miktarýný doðrudan azaltan veya arttýran bir etkisidir. Ýlk bölümde hasarýn hesaplanmasýnda etkilidir. Pozitif sayýlar hasarý arttýrýr, negatif sayýlar hasarý azaltýr</param>
    /// <param name="defenseHighestBuffMultiplier">Zýrh miktarýný oransal olarak arttýran bir etkidir. 0 ile 100 arasýnda bir deðer olmalýdýr. Varsayýlan olarak 0 belirlenmiþtir</param>
    /// <param name="defenseHighestDebuffMultiplier">Zýrh miktarýný oransal olarak azaltan bir etkidir. 0 ile 100 arasýnda bir deðer olmalýdýr. Varsayýlan olarak 0 belirlenmiþtir</param>
    /// <param name="maxArmorHalf">Zýrhýn maksimum deðerinin yarýsýdýr. Bu deðer ne kadar yüksek olursa zýrhýn etki o kadar az olacak, bir baþka deðiþle o kadar fazla hasar ortaya çýkacaktýr. 
    /// Mesela 100 olarak belirlenirse zýrh deðeri 100 olduðunda hasar yarý yarýya azalýr, 200 olduðunda hasarýn yarý yarýya azalmasý için zýrhýnda 200 olmasý gerekir.</param>
    /// <param name="defenseReduceOrPlus">Zýrh miktarýný doðrudan azaltan veya arttýran bir etkidir.</param>
    /// <param name="trueDamagePlusOrReduce">Bütün hesaplamardan baðýmsýz bir deðerdir, gerçek hasar olarak eklenir veya çýkartýlýr</param>
    /// <returns>float tipinde sayý döndürür, bu sayý hesaplanmýþ hasar miktarýdýr</returns>
    public float CalculateDamage(float characterAttackDamage, float abilityDamageRatio,
                                float enemyArmor, float criticRatio, float characterLevel, float enemyLevel,
                                float attackHighestBuffMultiplier = 0.0f, float attackHighestDebuffMultiplier = 0.0f,
                                float abilityDamageReduceRatio = 0.0f, float damageReduceOrPlus = 0.0f,
                                float defenseHighestBuffMultiplier = 0.0f, float defenseHighestDebuffMultiplier = 0.0f,
                                float maxArmorHalf = 500f, float defenseReduceOrPlus = 0.0f, int trueDamagePlusOrReduce = 0)
    {
        float calculatedDamage = Damage(characterAttackDamage, abilityDamageRatio, 
                                        attackHighestBuffMultiplier, attackHighestDebuffMultiplier,
                                        abilityDamageReduceRatio, damageReduceOrPlus) * Defense(enemyArmor, maxArmorHalf, defenseHighestBuffMultiplier,
                                                                                                defenseHighestDebuffMultiplier, defenseReduceOrPlus);
        if (criticRatio > 0)
        {
            return DamageWithCritic(calculatedDamage, criticRatio) * LevelDifference(characterLevel, enemyLevel) + trueDamagePlusOrReduce;
        }
        return calculatedDamage * LevelDifference(characterLevel, enemyLevel) + trueDamagePlusOrReduce;
    }


    /// <summary>
    /// Yetenek hasarý ile birlikte, zýrhýn koruma yüzdesi, kritik vurma þansý ve aradaki seviye farký dikkate alýnarak verilecek hasarý hesaplar. Seviye farký gözetilmez. 
    /// Yetenek hasarý temel saldýrý gücü baz alýnarak hesaplanýr
    /// Karþý tarafýn zýrh deðeri alýnarak hasar bir miktar azaltýlýr
    /// Kritik ile birlikte hasarý en az 2 katýna çýkrma ihtimali vardýr
    /// Ýlk önce karakterin saldýrý gücü ve yetenek oraný ile hesaplanýr, ardýndan zýrh korumasý ile bu hasarýn bir kýsmý azalýr, 
    /// sonra kritik þansý devreye girer ve karakterler arasýndaki seviye farký deðerlendirilir. En sonunda bütün bu hesaplamardan baðýmsýz gerçek hasar eklenir veya çýkartýlýr
    /// </summary>
    /// <param name="characterAttackDamage"> Karakterin eþyalarla birlikte sahip olduðu saldýrý gücüdür, pozitif sayýlar olmasý beklenir. Ne kadar büyük o kadar fazla hasar</param>
    /// <param name="abilityDamageRatio">Temel yetenek hasarýna yüzdelik ekstra güç ekler. Genellikle 0 ile 100 arasýnda bir deðer olmasý beklenir, 100'den fazla olabilir ama 0'dan az olmamalý. Örneðin temel yetenek hasarý 100, yetenek hasar oraný %10 toplam hasar 110</param>
    /// <param name="enemyArmor">Karþý tarafýn zýrhý, hasardan korunmayý saðlar. Ne kadar çok o kadar az hasar ama belli bir zýrhtan sonra hasar azaltma miktarý azalýr. Örneðin 100 zýrhta hasar yarýya düþüyorsa 200'de tamamen azalmaz</param>
    /// <param name="criticRatio">Kritik hasar ihtimali. 0 ile 100 arasýnda bir deðer olmalýdýr. Yüksek olmasý hem kritik olma ihtimalini arttýrýr, hem de hasar miktarýný artýrýr</param>
     /// <param name="attackHighestBuffMultiplier">Hasar miktrýný oransal olarak artýran bir buff etkisidir. 0 ile 100 arasýnda bir deðer olmalýdýr. Genellikle 0 ile 100 arasýnda bir deðer olmasý beklenir, 100'den fazla olabilir ama 0'dan az olmamalýdýr. Karþý tarafýn zýrhýndan etkilenir. Varsayýlan olarak 0 belirlenmiþtir</param>
    /// <param name="attackHighestDebuffMultiplier">Hasar miktarýný oransal olarak azaltan bir debuff etkisidir. Ýlk bölümde hasarýn hesaplanmasýnda etkilidir. 0 ile 100 arasýnda bir deðer olmalýdýr. Varsayýlan olarak 0 belirlenmiþtir</param>
    /// <param name="abilityDamageReduceRatio">Temel yetenek hasarýný oransal olarak azaltan bir debuff etkisidir. Ýlk bölümde hasarýn hesaplanmasýnda etkilidir. 0 ile 100 arasýnda bir deðer olmalýdýr. Varsayýlan olarak 0 belirlenmiþtir</param>
    /// <param name="damageReduceOrPlus">Hasar miktarýný doðrudan azaltan veya arttýran bir etkisidir. Ýlk bölümde hasarýn hesaplanmasýnda etkilidir. Pozitif sayýlar hasarý arttýrýr, negatif sayýlar hasarý azaltýr</param>
    /// <param name="defenseHighestBuffMultiplier">Zýrh miktarýný oransal olarak arttýran bir etkidir. 0 ile 100 arasýnda bir deðer olmalýdýr. Varsayýlan olarak 0 belirlenmiþtir</param>
    /// <param name="defenseHighestDebuffMultiplier">Zýrh miktarýný oransal olarak azaltan bir etkidir. 0 ile 100 arasýnda bir deðer olmalýdýr. Varsayýlan olarak 0 belirlenmiþtir</param>
    /// <param name="maxArmorHalf">Zýrhýn maksimum deðerinin yarýsýdýr. Bu deðer ne kadar yüksek olursa zýrhýn etki o kadar az olacak, bir baþka deðiþle o kadar fazla hasar ortaya çýkacaktýr. 
    /// Mesela 100 olarak belirlenirse zýrh deðeri 100 olduðunda hasar yarý yarýya azalýr, 200 olduðunda hasarýn yarý yarýya azalmasý için zýrhýnda 200 olmasý gerekir.</param>
    /// <param name="defenseReduceOrPlus">Zýrh miktarýný doðrudan azaltan veya arttýran bir etkidir.</param>
    /// <param name="trueDamagePlusOrReduce">Bütün hesaplamardan baðýmsýz bir deðerdir, gerçek hasar olarak eklenir veya çýkartýlýr</param>
    /// <returns>float tipinde sayý döndürür, bu sayý hesaplanmýþ hasar miktarýdýr</returns>
    public float CalculateDamageWithoutLevelDiff(float characterAttackDamage, float abilityDamageRatio,
                                float enemyArmor, float criticRatio,
                                float attackHighestBuffMultiplier = 0.0f, float attackHighestDebuffMultiplier = 0.0f,
                                float abilityDamageReduceRatio = 0.0f, float damageReduceOrPlus = 0.0f,
                                float defenseHighestBuffMultiplier = 0.0f, float defenseHighestDebuffMultiplier = 0.0f,
                                float maxArmorHalf = 500f, float defenseReduceOrPlus = 0.0f, int trueDamagePlusOrReduce = 0)
    {
        float calculatedDamage = Damage(characterAttackDamage, abilityDamageRatio,
                                        attackHighestBuffMultiplier, attackHighestDebuffMultiplier,
                                        abilityDamageReduceRatio, damageReduceOrPlus) * Defense(enemyArmor, maxArmorHalf, defenseHighestBuffMultiplier,
                                                                                                defenseHighestDebuffMultiplier, defenseReduceOrPlus);
        if (criticRatio > 0)
        {
            return DamageWithCritic(calculatedDamage, criticRatio) + trueDamagePlusOrReduce;
        }
        return calculatedDamage + trueDamagePlusOrReduce;
    }

}
