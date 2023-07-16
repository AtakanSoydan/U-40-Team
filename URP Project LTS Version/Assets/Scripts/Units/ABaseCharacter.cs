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
    /// Kritik hasar�n hesaplanmas�. Temel hasar� en az 2 kat artt�r�r. Belli bir olas�l�k ile ger�ekle�ebilir, e�er ger�ekle�mezse temel hasar miktar�n� d�nd�r�r
    /// </summary>
    /// <param name="calculatedDamage">Temel hasar miktar�</param>
    /// <param name="criticRatio">Kritik hasar ihtimali. 0 ile 100 aras�nda bir de�er olmal�d�r. Y�ksek olmas� hem kritik olma ihtimalini artt�r�r, hem de hasar miktar�n� art�r�r</param>
    /// <returns>�htimal ger�ekle�irse kritik hasarla birlikte, ger�ekle�mezse temel hasar miktar�n� float cinsinden d�nd�r�r</returns>
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
    /// Z�rh ile hasar�n % ka��ndan korunulaca�� hesaplan�r
    /// </summary>
    /// <param name="enemyArmor">Kar�� taraf�n z�rh�, hasardan korunmay� sa�lar. Ne kadar �ok o kadar az hasar ama belli bir z�rhtan sonra hasar azaltma miktar� azal�r. �rne�in 100 z�rhta hasar yar�ya d���yorsa 200'de tamamen azalmaz</param>
    /// <param name="maxArmorHalf">Z�rh�n maksimum de�erinin yar�s�d�r. Bu de�er ne kadar y�ksek olursa z�rh�n etki o kadar az olacak, bir ba�ka de�i�le o kadar fazla hasar ortaya ��kacakt�r. 
    /// Mesela 100 olarak belirlenirse z�rh de�eri 100 oldu�unda hasar yar� yar�ya azal�r, 200 oldu�unda hasar�n yar� yar�ya azalmas� i�in z�rh�nda 200 olmas� gerekir.</param>
    /// <param name="defenseHighestBuffMultiplier">Z�rh miktar�n� oransal olarak artt�ran bir etkidir. 0 ile 100 aras�nda bir de�er olmal�d�r. Varsay�lan olarak 0 belirlenmi�tir</param>
    /// <param name="defenseHighestDebuffMultiplier">Z�rh miktar�n� oransal olarak azaltan bir etkidir. 0 ile 100 aras�nda bir de�er olmal�d�r. Varsay�lan olarak 0 belirlenmi�tir</param>
    /// <param name="defenseReduceOrPlus">Z�rh miktar�n� do�rudan azaltan veya artt�ran bir etkidir.</param>
    /// <returns> Normal ko�ullarda 0 ile 1 aras�nda bir de�er d�nd�r�r</returns>
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
    /// Karakterler aras�ndaki seviye fark� de�erlendirilir
    /// </summary>
    /// <param name="characterLevel">Karakterin seviyesidir. 0 ile 100 aras�nda bir de�er olmas� beklenir</param>
    /// <param name="enemyLevel">Kar�� taraf�n seviyesidir. 0 ile 100 aras�nda bir de�er olmas� beklenir</param>
    /// <returns> Normal ko�ullarda 1 ile 2 aras�nda bir de�er d�nd�rmesi beklenir</returns>
    public float LevelDifference(float characterLevel, float enemyLevel)
    {
        return (1 + (characterLevel / 200) - (enemyLevel / 200));
    }


    /// <summary>
    /// Yetenek hasar� ile birlikte, z�rh�n koruma y�zdesi, kritik vurma �ans� ve aradaki seviye fark� dikkate al�narak verilecek hasar� hesaplar. 
    /// Yetenek hasar� temel hasardan �st�ne ekstra olarak eklenir
    /// Kar�� taraf�n z�rh de�eri al�narak hasar bir miktar azalt�l�r
    /// Kritik ile birlikte hasar� en az 2 kat�na ��krma ihtimali vard�r
    /// Seviye fark�na g�re hasar azalabilir veya artabilir
    /// �lk �nce karakterin sald�r� g�c� ve yetenek hasar� hesaplan�r, ard�ndan z�rh korumas� ile bu hasar�n bir k�sm� azal�r, 
    /// sonra kritik �ans� devreye girer ve karakterler aras�ndaki seviye fark� de�erlendirilir. En sonunda b�t�n bu hesaplamardan ba��ms�z ger�ek hasar eklenir veya ��kart�l�r
    /// </summary>
    /// <param name="characterAttackDamage"> Karakterin e�yalarla birlikte sahip oldu�u sald�r� g�c�d�r, pozitif say�lar olmas� beklenir. Ne kadar b�y�k o kadar fazla hasar</param>
    /// <param name="baseAbilityDamage"> Karakter g�c�nden ba��ms�z ekstra temel yetenek hasar�, pozitif say�lar olmas� beklenir </param>
    /// <param name="abilityDamageRatio">Temel yetenek hasar�na y�zdelik ekstra g�� ekler. Genellikle 0 ile 100 aras�nda bir de�er olmas� beklenir, 100'den fazla olabilir ama 0'dan az olmamal�. �rne�in temel yetenek hasar� 100, yetenek hasar oran� %10 toplam hasar 110</param>
    /// <param name="enemyArmor">Kar�� taraf�n z�rh�, hasardan korunmay� sa�lar. Ne kadar �ok o kadar az hasar ama belli bir z�rhtan sonra hasar azaltma miktar� azal�r. �rne�in 100 z�rhta hasar yar�ya d���yorsa 200'de tamamen azalmaz</param>
    /// <param name="criticRatio">Kritik hasar ihtimali. 0 ile 100 aras�nda bir de�er olmal�d�r. Y�ksek olmas� hem kritik olma ihtimalini artt�r�r, hem de hasar miktar�n� art�r�r</param>
    /// <param name="characterLevel"> Sald�r� yapan�n seviyesi. Kar�� taraf�n seviyesinden y�ksekse daha fazla hasar d���kse daha az hasar ortaya ��kar</param>
    /// <param name="enemyLevel">Sald�r� hedefinin seviyesi. Sald�r� yapan�n seviyesinden y�ksekse daha az hasar d���kse daha fazla hasar ortaya ��kar</param>
    /// <param name="attackHighestBuffMultiplier">Hasar miktr�n� oransal olarak art�ran bir buff etkisidir. 0 ile 100 aras�nda bir de�er olmal�d�r. Genellikle 0 ile 100 aras�nda bir de�er olmas� beklenir, 100'den fazla olabilir ama 0'dan az olmamal�d�r. Kar�� taraf�n z�rh�ndan etkilenir. Varsay�lan olarak 0 belirlenmi�tir</param>
    /// <param name="attackHighestDebuffMultiplier">Hasar miktar�n� oransal olarak azaltan bir debuff etkisidir. �lk b�l�mde hasar�n hesaplanmas�nda etkilidir. 0 ile 100 aras�nda bir de�er olmal�d�r. Varsay�lan olarak 0 belirlenmi�tir</param>
    /// <param name="abilityDamageReduceRatio">Temel yetenek hasar�n� oransal olarak azaltan bir debuff etkisidir. �lk b�l�mde hasar�n hesaplanmas�nda etkilidir. 0 ile 100 aras�nda bir de�er olmal�d�r. Varsay�lan olarak 0 belirlenmi�tir</param>
    /// <param name="damageReduceOrPlus">Hasar miktar�n� do�rudan azaltan veya artt�ran bir etkisidir. �lk b�l�mde hasar�n hesaplanmas�nda etkilidir. Pozitif say�lar hasar� artt�r�r, negatif say�lar hasar� azalt�r</param>
    /// <param name="defenseHighestBuffMultiplier">Z�rh miktar�n� oransal olarak artt�ran bir etkidir. 0 ile 100 aras�nda bir de�er olmal�d�r. Varsay�lan olarak 0 belirlenmi�tir</param>
    /// <param name="defenseHighestDebuffMultiplier">Z�rh miktar�n� oransal olarak azaltan bir etkidir. 0 ile 100 aras�nda bir de�er olmal�d�r. Varsay�lan olarak 0 belirlenmi�tir</param>
    /// <param name="maxArmorHalf">Z�rh�n maksimum de�erinin yar�s�d�r. Bu de�er ne kadar y�ksek olursa z�rh�n etki o kadar az olacak, bir ba�ka de�i�le o kadar fazla hasar ortaya ��kacakt�r. 
    /// Mesela 100 olarak belirlenirse z�rh de�eri 100 oldu�unda hasar yar� yar�ya azal�r, 200 oldu�unda hasar�n yar� yar�ya azalmas� i�in z�rh�nda 200 olmas� gerekir.</param>
    /// <param name="defenseReduceOrPlus">Z�rh miktar�n� do�rudan azaltan veya artt�ran bir etkidir.</param>
    /// <param name="trueDamagePlusOrReduce">B�t�n hesaplamardan ba��ms�z bir de�erdir, ger�ek hasar olarak eklenir veya ��kart�l�r</param>
    /// <returns>float tipinde say� d�nd�r�r, bu say� hesaplanm�� hasar miktar�d�r</returns>
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
    /// Yetenek olmaks�z�n karakterin sald�r� g�c�yle verilen temel sald�r� hasar�n� hesaplar
    /// Kar�� taraf�n z�rh de�eri al�narak hasar bir miktar azalt�l�r
    /// Kritik ile birlikte hasar� en az 2 kat�na ��krma ihtimali vard�r
    /// Seviye fark�na g�re hasar azalabilir veya artabilir
    /// �lk �nce karakterin sald�r� g�c� ile bir hasar hesaplan�r, ard�ndan z�rh korumas� ile bu hasar�n bir k�sm� azal�r, 
    /// sonra kritik �ans� devreye girer ve karakterler aras�ndaki seviye fark� de�erlendirilir. En sonunda b�t�n bu hesaplamardan ba��ms�z ger�ek hasar eklenir veya ��kart�l�r
    /// </summary>
    /// <param name="characterAttackDamage"> Karakterin e�yalarla birlikte sahip oldu�u sald�r� g�c�d�r, pozitif say�lar olmas� beklenir. Ne kadar b�y�k o kadar fazla hasar</param>
    /// <param name="enemyArmor">Kar�� taraf�n z�rh�, hasardan korunmay� sa�lar. Ne kadar �ok o kadar az hasar ama belli bir z�rhtan sonra hasar azaltma miktar� azal�r. �rne�in 100 z�rhta hasar yar�ya d���yorsa 200'de tamamen azalmaz</param>
    /// <param name="criticRatio">Kritik hasar ihtimali. 0 ile 100 aras�nda bir de�er olmal�d�r. Y�ksek olmas� hem kritik olma ihtimalini artt�r�r, hem de hasar miktar�n� art�r�r</param>
    /// <param name="characterLevel"> Sald�r� yapan�n seviyesi. Kar�� taraf�n seviyesinden y�ksekse daha fazla hasar d���kse daha az hasar ortaya ��kar</param>
    /// <param name="enemyLevel">Sald�r� hedefinin seviyesi. Sald�r� yapan�n seviyesinden y�ksekse daha az hasar d���kse daha fazla hasar ortaya ��kar</param>
    /// <param name="attackHighestBuffMultiplier">Hasar miktr�n� oransal olarak art�ran bir buff etkisidir. 0 ile 100 aras�nda bir de�er olmal�d�r. Genellikle 0 ile 100 aras�nda bir de�er olmas� beklenir, 100'den fazla olabilir ama 0'dan az olmamal�d�r. Kar�� taraf�n z�rh�ndan etkilenir. Varsay�lan olarak 0 belirlenmi�tir</param>
    /// <param name="attackHighestDebuffMultiplier">Hasar miktar�n� oransal olarak azaltan bir debuff etkisidir. �lk b�l�mde hasar�n hesaplanmas�nda etkilidir. 0 ile 100 aras�nda bir de�er olmal�d�r. Varsay�lan olarak 0 belirlenmi�tir</param>
    /// <param name="abilityDamageReduceRatio">Temel yetenek hasar�n� oransal olarak azaltan bir debuff etkisidir. �lk b�l�mde hasar�n hesaplanmas�nda etkilidir. 0 ile 100 aras�nda bir de�er olmal�d�r. Varsay�lan olarak 0 belirlenmi�tir</param>
    /// <param name="damageReduceOrPlus">Hasar miktar�n� do�rudan azaltan veya artt�ran bir etkisidir. �lk b�l�mde hasar�n hesaplanmas�nda etkilidir. Pozitif say�lar hasar� artt�r�r, negatif say�lar hasar� azalt�r</param>
    /// <param name="defenseHighestBuffMultiplier">Z�rh miktar�n� oransal olarak artt�ran bir etkidir. 0 ile 100 aras�nda bir de�er olmal�d�r. Varsay�lan olarak 0 belirlenmi�tir</param>
    /// <param name="defenseHighestDebuffMultiplier">Z�rh miktar�n� oransal olarak azaltan bir etkidir. 0 ile 100 aras�nda bir de�er olmal�d�r. Varsay�lan olarak 0 belirlenmi�tir</param>
    /// <param name="maxArmorHalf">Z�rh�n maksimum de�erinin yar�s�d�r. Bu de�er ne kadar y�ksek olursa z�rh�n etki o kadar az olacak, bir ba�ka de�i�le o kadar fazla hasar ortaya ��kacakt�r. 
    /// Mesela 100 olarak belirlenirse z�rh de�eri 100 oldu�unda hasar yar� yar�ya azal�r, 200 oldu�unda hasar�n yar� yar�ya azalmas� i�in z�rh�nda 200 olmas� gerekir.</param>
    /// <param name="defenseReduceOrPlus">Z�rh miktar�n� do�rudan azaltan veya artt�ran bir etkidir.</param>
    /// <param name="trueDamagePlusOrReduce">B�t�n hesaplamardan ba��ms�z bir de�erdir, ger�ek hasar olarak eklenir veya ��kart�l�r</param>
    /// <returns>float tipinde say� d�nd�r�r, bu say� hesaplanm�� hasar miktar�d�r</returns>
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
    /// Yetenek hasar� ile birlikte, z�rh�n koruma y�zdesi, kritik vurma �ans� ve aradaki seviye fark� dikkate al�narak verilecek hasar� hesaplar. 
    /// Yetenek hasar� temel sald�r� g�c� baz al�narak hesaplan�r
    /// Kar�� taraf�n z�rh de�eri al�narak hasar bir miktar azalt�l�r
    /// Kritik ile birlikte hasar� en az 2 kat�na ��krma ihtimali vard�r
    /// Seviye fark�na g�re hasar azalabilir veya artabilir
    /// �lk �nce karakterin sald�r� g�c� ve yetenek oran� ile hesaplan�r, ard�ndan z�rh korumas� ile bu hasar�n bir k�sm� azal�r, 
    /// sonra kritik �ans� devreye girer ve karakterler aras�ndaki seviye fark� de�erlendirilir. En sonunda b�t�n bu hesaplamardan ba��ms�z ger�ek hasar eklenir veya ��kart�l�r
    /// </summary>
    /// <param name="characterAttackDamage"> Karakterin e�yalarla birlikte sahip oldu�u sald�r� g�c�d�r, pozitif say�lar olmas� beklenir. Ne kadar b�y�k o kadar fazla hasar</param>
    /// <param name="abilityDamageRatio">Temel yetenek hasar�na y�zdelik ekstra g�� ekler. Genellikle 0 ile 100 aras�nda bir de�er olmas� beklenir, 100'den fazla olabilir ama 0'dan az olmamal�. �rne�in temel yetenek hasar� 100, yetenek hasar oran� %10 toplam hasar 110</param>
    /// <param name="enemyArmor">Kar�� taraf�n z�rh�, hasardan korunmay� sa�lar. Ne kadar �ok o kadar az hasar ama belli bir z�rhtan sonra hasar azaltma miktar� azal�r. �rne�in 100 z�rhta hasar yar�ya d���yorsa 200'de tamamen azalmaz</param>
    /// <param name="criticRatio">Kritik hasar ihtimali. 0 ile 100 aras�nda bir de�er olmal�d�r. Y�ksek olmas� hem kritik olma ihtimalini artt�r�r, hem de hasar miktar�n� art�r�r</param>
    /// <param name="characterLevel"> Sald�r� yapan�n seviyesi. Kar�� taraf�n seviyesinden y�ksekse daha fazla hasar d���kse daha az hasar ortaya ��kar</param>
    /// <param name="enemyLevel">Sald�r� hedefinin seviyesi. Sald�r� yapan�n seviyesinden y�ksekse daha az hasar d���kse daha fazla hasar ortaya ��kar</param>
    /// <param name="attackHighestBuffMultiplier">Hasar miktr�n� oransal olarak art�ran bir buff etkisidir. 0 ile 100 aras�nda bir de�er olmal�d�r. Genellikle 0 ile 100 aras�nda bir de�er olmas� beklenir, 100'den fazla olabilir ama 0'dan az olmamal�d�r. Kar�� taraf�n z�rh�ndan etkilenir. Varsay�lan olarak 0 belirlenmi�tir</param>
    /// <param name="attackHighestDebuffMultiplier">Hasar miktar�n� oransal olarak azaltan bir debuff etkisidir. �lk b�l�mde hasar�n hesaplanmas�nda etkilidir. 0 ile 100 aras�nda bir de�er olmal�d�r. Varsay�lan olarak 0 belirlenmi�tir</param>
    /// <param name="abilityDamageReduceRatio">Temel yetenek hasar�n� oransal olarak azaltan bir debuff etkisidir. �lk b�l�mde hasar�n hesaplanmas�nda etkilidir. 0 ile 100 aras�nda bir de�er olmal�d�r. Varsay�lan olarak 0 belirlenmi�tir</param>
    /// <param name="damageReduceOrPlus">Hasar miktar�n� do�rudan azaltan veya artt�ran bir etkisidir. �lk b�l�mde hasar�n hesaplanmas�nda etkilidir. Pozitif say�lar hasar� artt�r�r, negatif say�lar hasar� azalt�r</param>
    /// <param name="defenseHighestBuffMultiplier">Z�rh miktar�n� oransal olarak artt�ran bir etkidir. 0 ile 100 aras�nda bir de�er olmal�d�r. Varsay�lan olarak 0 belirlenmi�tir</param>
    /// <param name="defenseHighestDebuffMultiplier">Z�rh miktar�n� oransal olarak azaltan bir etkidir. 0 ile 100 aras�nda bir de�er olmal�d�r. Varsay�lan olarak 0 belirlenmi�tir</param>
    /// <param name="maxArmorHalf">Z�rh�n maksimum de�erinin yar�s�d�r. Bu de�er ne kadar y�ksek olursa z�rh�n etki o kadar az olacak, bir ba�ka de�i�le o kadar fazla hasar ortaya ��kacakt�r. 
    /// Mesela 100 olarak belirlenirse z�rh de�eri 100 oldu�unda hasar yar� yar�ya azal�r, 200 oldu�unda hasar�n yar� yar�ya azalmas� i�in z�rh�nda 200 olmas� gerekir.</param>
    /// <param name="defenseReduceOrPlus">Z�rh miktar�n� do�rudan azaltan veya artt�ran bir etkidir.</param>
    /// <param name="trueDamagePlusOrReduce">B�t�n hesaplamardan ba��ms�z bir de�erdir, ger�ek hasar olarak eklenir veya ��kart�l�r</param>
    /// <returns>float tipinde say� d�nd�r�r, bu say� hesaplanm�� hasar miktar�d�r</returns>
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
    /// Yetenek hasar� ile birlikte, z�rh�n koruma y�zdesi, kritik vurma �ans� ve aradaki seviye fark� dikkate al�narak verilecek hasar� hesaplar. Seviye fark� g�zetilmez. 
    /// Yetenek hasar� temel sald�r� g�c� baz al�narak hesaplan�r
    /// Kar�� taraf�n z�rh de�eri al�narak hasar bir miktar azalt�l�r
    /// Kritik ile birlikte hasar� en az 2 kat�na ��krma ihtimali vard�r
    /// �lk �nce karakterin sald�r� g�c� ve yetenek oran� ile hesaplan�r, ard�ndan z�rh korumas� ile bu hasar�n bir k�sm� azal�r, 
    /// sonra kritik �ans� devreye girer ve karakterler aras�ndaki seviye fark� de�erlendirilir. En sonunda b�t�n bu hesaplamardan ba��ms�z ger�ek hasar eklenir veya ��kart�l�r
    /// </summary>
    /// <param name="characterAttackDamage"> Karakterin e�yalarla birlikte sahip oldu�u sald�r� g�c�d�r, pozitif say�lar olmas� beklenir. Ne kadar b�y�k o kadar fazla hasar</param>
    /// <param name="abilityDamageRatio">Temel yetenek hasar�na y�zdelik ekstra g�� ekler. Genellikle 0 ile 100 aras�nda bir de�er olmas� beklenir, 100'den fazla olabilir ama 0'dan az olmamal�. �rne�in temel yetenek hasar� 100, yetenek hasar oran� %10 toplam hasar 110</param>
    /// <param name="enemyArmor">Kar�� taraf�n z�rh�, hasardan korunmay� sa�lar. Ne kadar �ok o kadar az hasar ama belli bir z�rhtan sonra hasar azaltma miktar� azal�r. �rne�in 100 z�rhta hasar yar�ya d���yorsa 200'de tamamen azalmaz</param>
    /// <param name="criticRatio">Kritik hasar ihtimali. 0 ile 100 aras�nda bir de�er olmal�d�r. Y�ksek olmas� hem kritik olma ihtimalini artt�r�r, hem de hasar miktar�n� art�r�r</param>
     /// <param name="attackHighestBuffMultiplier">Hasar miktr�n� oransal olarak art�ran bir buff etkisidir. 0 ile 100 aras�nda bir de�er olmal�d�r. Genellikle 0 ile 100 aras�nda bir de�er olmas� beklenir, 100'den fazla olabilir ama 0'dan az olmamal�d�r. Kar�� taraf�n z�rh�ndan etkilenir. Varsay�lan olarak 0 belirlenmi�tir</param>
    /// <param name="attackHighestDebuffMultiplier">Hasar miktar�n� oransal olarak azaltan bir debuff etkisidir. �lk b�l�mde hasar�n hesaplanmas�nda etkilidir. 0 ile 100 aras�nda bir de�er olmal�d�r. Varsay�lan olarak 0 belirlenmi�tir</param>
    /// <param name="abilityDamageReduceRatio">Temel yetenek hasar�n� oransal olarak azaltan bir debuff etkisidir. �lk b�l�mde hasar�n hesaplanmas�nda etkilidir. 0 ile 100 aras�nda bir de�er olmal�d�r. Varsay�lan olarak 0 belirlenmi�tir</param>
    /// <param name="damageReduceOrPlus">Hasar miktar�n� do�rudan azaltan veya artt�ran bir etkisidir. �lk b�l�mde hasar�n hesaplanmas�nda etkilidir. Pozitif say�lar hasar� artt�r�r, negatif say�lar hasar� azalt�r</param>
    /// <param name="defenseHighestBuffMultiplier">Z�rh miktar�n� oransal olarak artt�ran bir etkidir. 0 ile 100 aras�nda bir de�er olmal�d�r. Varsay�lan olarak 0 belirlenmi�tir</param>
    /// <param name="defenseHighestDebuffMultiplier">Z�rh miktar�n� oransal olarak azaltan bir etkidir. 0 ile 100 aras�nda bir de�er olmal�d�r. Varsay�lan olarak 0 belirlenmi�tir</param>
    /// <param name="maxArmorHalf">Z�rh�n maksimum de�erinin yar�s�d�r. Bu de�er ne kadar y�ksek olursa z�rh�n etki o kadar az olacak, bir ba�ka de�i�le o kadar fazla hasar ortaya ��kacakt�r. 
    /// Mesela 100 olarak belirlenirse z�rh de�eri 100 oldu�unda hasar yar� yar�ya azal�r, 200 oldu�unda hasar�n yar� yar�ya azalmas� i�in z�rh�nda 200 olmas� gerekir.</param>
    /// <param name="defenseReduceOrPlus">Z�rh miktar�n� do�rudan azaltan veya artt�ran bir etkidir.</param>
    /// <param name="trueDamagePlusOrReduce">B�t�n hesaplamardan ba��ms�z bir de�erdir, ger�ek hasar olarak eklenir veya ��kart�l�r</param>
    /// <returns>float tipinde say� d�nd�r�r, bu say� hesaplanm�� hasar miktar�d�r</returns>
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
