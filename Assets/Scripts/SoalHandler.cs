using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class SoalHandler : MonoBehaviour
{
    private GameLoger gl;


    public float sumKali;

    public int jumlahBlok;
    public int jumlahOperand;
    public List<float> ListBilangan;


    public float curDifficulty = 0f;

    private Generator gen;


    private int jumlahTambah = 0;
    private int jumlahKurang = 0;
    private int jumlahKali = 0;
    private int jumlahBagi = 0;
    public int lifeCount = 0;

    public string solusi = "";

    private ScoreLoger sl;



    public Soal newCandidate(List<Bilangan> currBalok, Node currTargt, string currSolution, Difficulty currDifficulty)
    {
        Soal temp = new Soal();
        temp.balok = currBalok;
        temp.target = currTargt;
        temp.solusi = currSolution;
        temp.difficulty = currDifficulty;
        return temp;
    }

    public Difficulty newDifficulty(float z1, float z2, float z3, float z4, float pdiff, float wdiff)
    {
        Difficulty temp = new Difficulty();
        temp.z1 = z1;
        temp.z2 = z2;
        temp.z3 = z3;
        temp.z4 = z4;
        temp.pdifficulty = pdiff;
        temp.wdifficulty = wdiff;
        return temp;
    }

    private void Start()
    {
        System.Diagnostics.Stopwatch stAll = new System.Diagnostics.Stopwatch();
        stAll.Start();
        
        sl = GameObject.Find("ScoreLoger").GetComponent<ScoreLoger>();
        gl = GameObject.Find("GameLoger").GetComponent<GameLoger>();
        gl.indexSoal = gl.indexSoal + 1; // init index soal

        if(gl.indexSoal == 1)
        {
            gl.targetDifficulty = 0.5f;
        }

        //jumlahBlok = jumlahTambah + jumlahKurang + jumlahKali + jumlahBagi;

        gen = this.gameObject.GetComponent<Generator>();

        bool founded = false;
        int cobaCount = 0;

        if (gl.prevStatus == "SUCCESS" || gl.prevStatus == "SKIPPED")
        {
            // if(sl.jumlahKerjakanSoal == 0){
            //     Soal soalCandidate = newCandidate(null, null,"", newDifficulty(0f,0f,0f,0f,0f,2f)); //w = 2f
            // }else{
            //     Soal soalCandidate = newCandidate(null, null,"", newDifficulty(0f,0f,0f,0f,0f,2f)); //w = 2f
            // }
            Soal soalCandidate = newCandidate(null, null,"", newDifficulty(0f,0f,0f,0f,0f,2f) ); //w = 2f
            while (!founded)
            {


                Debug.Log("-RESTART-");

                //System.Diagnostics.Stopwatch stGenerator = new System.Diagnostics.Stopwatch();
                //stGenerator.Start();

                generateMainBlok();

                //stGenerator.Stop();
                //Debug.LogError("Generator: " + (stGenerator.Elapsed));
                //stGenerator.Reset();

                Debug.LogWarning("blok: " + (jumlahBagi + jumlahKali + jumlahKurang + jumlahTambah));
                List<Bilangan> bilanganObj = BuatSoal();
                Difficulty difficultyIndex = getDifficulty(bilanganObj);
                SetLife(difficultyIndex.wdifficulty);
                Node target = getTarget(bilanganObj);

                //Debug.LogWarning(jumlahBlok);
                //Debug.LogWarning(jumlahOperand);
                Debug.Log("curDiff: " + difficultyIndex.wdifficulty);
                float q = gl.prevDifficulty + gl.prevPerformance;
                Debug.Log("target: " + q);

                if (target == null)
                {
                    Debug.LogWarning("TARGET NULL");
                }
                else
                {
                    cobaCount++;
                    //float minTreshold = gl.prevDifficulty - 0.015f; //0.447 || 0.015
                    //float maxTreshold = gl.prevDifficulty - 0.1f; // 0.362  -   0.462 || 0.1

                    string aS = difficultyIndex.wdifficulty.ToString().Substring(0, 3); //1 angka dibelakang koma (0.5..)
                    string bS = gl.targetDifficulty.ToString().Substring(0, 3);
                    bool optimized = false;

                    //Debug.LogWarning("beda curr: " + Mathf.Abs(difficultyIndex - gl.targetDifficulty));
                    //Debug.LogWarning("beda best: " + Mathf.Abs(soalCandidate.difficulty - gl.targetDifficulty));
                    Debug.LogWarning("curr: " + difficultyIndex.wdifficulty);

                    if (soalCandidate.balok == null)
                    {
                        soalCandidate = newCandidate(new List<Bilangan>(bilanganObj), target,solusi, newDifficulty(difficultyIndex.z1, difficultyIndex.z2, difficultyIndex.z3, difficultyIndex.z4, difficultyIndex.pdifficulty, difficultyIndex.wdifficulty));
                    }
                    if (Mathf.Abs(difficultyIndex.wdifficulty - gl.targetDifficulty) < Mathf.Abs(soalCandidate.difficulty.wdifficulty - gl.targetDifficulty))
                    {
                        soalCandidate = null;
                        soalCandidate = newCandidate(new List<Bilangan>(bilanganObj), target, solusi, newDifficulty(difficultyIndex.z1, difficultyIndex.z2, difficultyIndex.z3, difficultyIndex.z4, difficultyIndex.pdifficulty, difficultyIndex.wdifficulty));

                    }

                    if (aS == bS)
                    {
                        int optimizeCount = 0;
                        while (!optimized)
                        {
                            cobaCount++;
                            optimizeCount++;
                            target = null;
                            bilanganObj.Clear();
                            bilanganObj = BuatSoal();
                            difficultyIndex = getDifficulty(bilanganObj);
                            SetLife(difficultyIndex.wdifficulty);
                            target = getTarget(bilanganObj);
                            if (target == null)
                            {
                                Debug.LogWarning("TARGET NULL");
                            }
                            else
                            {
                                float targetDiff = 0;
                                string diffstring = "";
                                string cutted = "";

                                //float maxTreshold = 0;
                                //float minTreshold = 0;

                                if (gl.prevStatus == "SKIPPED")
                                {
                                    targetDiff = gl.prevDifficulty - gl.prevPerformance;
                                    diffstring = targetDiff.ToString();
                                    cutted = diffstring.Substring(0, 4);

                                    //maxTreshold = float.Parse(cutted);
                                    //minTreshold = maxTreshold - 0.01f;
                                }
                                else if (gl.prevStatus == "SUCCESS")
                                {
                                    targetDiff = gl.prevDifficulty + gl.prevPerformance;
                                    diffstring = targetDiff.ToString();
                                    cutted = diffstring.Substring(0, 4);

                                    //maxTreshold = float.Parse(cutted);
                                    //minTreshold = maxTreshold + 0.01f;
                                }
                                Debug.LogWarning("OPTIMIZING..."); //TODO: optimizing ini bisa jadi dia capek nyari, karna bisa jadi memang kombinasi yang saat ini ga bisa bentuk difficulty segitu;
                                Debug.LogWarning(jumlahBlok);
                                Debug.LogWarning(jumlahOperand);
                                Debug.Log("curDiff: " + difficultyIndex);
                                Debug.Log("prevDiff: " + gl.prevDifficulty);
                                Debug.Log("prevformance: " + gl.prevPerformance);
                                Debug.Log("target: " + targetDiff);
                                //Debug.Log("minTres: " + minTreshold);
                                //Debug.Log("maxTres: " + maxTreshold);


                                //Debug.LogWarning("beda curr: " + Mathf.Abs(difficultyIndex - gl.targetDifficulty));
                                //Debug.LogWarning("beda best: " + Mathf.Abs(soalCandidate.difficulty - gl.targetDifficulty));
                                Debug.LogWarning("curr: " + difficultyIndex);
                                Debug.LogWarning("best: " + soalCandidate.difficulty);

                                //get soal paling mendekati
                                if (Mathf.Abs(difficultyIndex.wdifficulty - gl.targetDifficulty) < Mathf.Abs(soalCandidate.difficulty.wdifficulty - gl.targetDifficulty))
                                {
                                    soalCandidate = null;
                                    soalCandidate = newCandidate(new List<Bilangan>(bilanganObj), target,solusi, newDifficulty(difficultyIndex.z1, difficultyIndex.z2, difficultyIndex.z3, difficultyIndex.z4, difficultyIndex.pdifficulty, difficultyIndex.wdifficulty));
                                }


                                if (optimizeCount >= 500)
                                {
                                    // Debug.Log("COBA COUNT: " + cobaCount);
                                    gl.isFound = true;
                                    gl.difficulty = soalCandidate.difficulty.wdifficulty; // LOG
                                    solusi = soalCandidate.solusi;
                                    GenerateSoal(soalCandidate.balok, soalCandidate.target);

                                    string soal = "";
                                    foreach (Bilangan item in soalCandidate.balok)
                                    {
                                        soal += (item.bilangan.ToString() + item.op.ToString() + "|");
                                    }


                                    gl.z1 = soalCandidate.difficulty.z1;
                                    Debug.LogError("TES: " + gl.z1);
                                    gl.z2 = soalCandidate.difficulty.z2;
                                    gl.z3 = soalCandidate.difficulty.z3;
                                    gl.z4 = soalCandidate.difficulty.z4;
                                    gl.pureDiff = soalCandidate.difficulty.pdifficulty;

                                    gl.blokSoal = soal;
                                    gl.solusiSoal = solusi;

                                    founded = true;
                                    optimized = true;
                                    Debug.LogWarning("Optimizing selesai. semua selesai");
                                }
                            }


                        }
                    }
                    if (cobaCount >= 1000)
                    {
                        founded = true;
                        // GenerateSoal(bilanganObj, target);
                        //Debug.Log("balok keluar di diff ini");
                        //foreach (Bilangan item in soalCandidate.balok)
                        //{
                        //    Debug.Log(item.bilangan + item.op);
                        //}
                        solusi = soalCandidate.solusi;
                        GenerateSoal(soalCandidate.balok, soalCandidate.target);

                        //LOG
                        string soal = "";
                        foreach (Bilangan item in soalCandidate.balok)
                        {
                            soal += (item.bilangan.ToString() + item.op.ToString() + "|");
                        }
                        gl.blokSoal = soal;
                        gl.solusiSoal = solusi;
                        gl.z1 = soalCandidate.difficulty.z1;
                        gl.z2 = soalCandidate.difficulty.z2;
                        gl.z3 = soalCandidate.difficulty.z3;
                        gl.z4 = soalCandidate.difficulty.z4;
                        gl.pureDiff = soalCandidate.difficulty.pdifficulty;
                        gl.difficulty = soalCandidate.difficulty.wdifficulty; // LOG
                        gl.isFound = false; // LOG
                        
                        //Debug.Log("set diff: " + gl.difficulty);
                        SetLife(difficultyIndex.wdifficulty); //hati
                        //Debug.Log("Best diff: " + soalCandidate.difficulty);
                        Debug.LogWarning("best: " + soalCandidate.difficulty);
                        Debug.LogWarning("CAPEK NYARI SUMPAH");
                        break;
                    }


                }



            }
            stAll.Stop();
            Debug.LogError("Time taken: " + (stAll.Elapsed));
            gl.elespasedTime = (((float) stAll.Elapsed.Seconds) + ((float) stAll.Elapsed.Milliseconds)/1000).ToString();
            stAll.Reset();
        }
        else
        {


            //Debug.LogError("-AWAL-");
            while (!founded)
            {


                cobaCount++;

                //System.Diagnostics.Stopwatch stGenerator = new System.Diagnostics.Stopwatch();
                //stGenerator.Start();

                generateMainBlok();

                //stGenerator.Stop();
                //Debug.LogError("Generator: " + (stGenerator.Elapsed));
                //stGenerator.Reset();

                List<Bilangan> bilanganObj = BuatSoal();
                Difficulty difficultyIndex = getDifficulty(bilanganObj);
                SetLife(difficultyIndex.wdifficulty);
                Node target = getTarget(bilanganObj);
                if (target == null)
                {
                    Debug.LogWarning("TARGET NULL");
                }
                else
                {
                    if (cobaCount <= 1000)
                    {
                        GenerateSoal(bilanganObj, target);

                        string soal = "";
                        foreach (Bilangan item in bilanganObj)
                        {
                            soal += (item.bilangan.ToString() + item.op.ToString()+"|");
                        }
                        gl.blokSoal = soal;
                        gl.solusiSoal = solusi;

                        founded = true;

                    }
                    else
                    {
                        Debug.LogWarning("INITATE GALGAL 1000x berturut turut, gila");
                    }


                }



            }
            stAll.Stop();
            Debug.LogError("Time taken: " + (stAll.Elapsed));
            gl.elespasedTime = (((float) stAll.Elapsed.Seconds) + ((float) stAll.Elapsed.Milliseconds)/1000).ToString();
            stAll.Reset();
        }
    }

    private void SetLife(float difficultyIndex)
    {
        string cuted = difficultyIndex.ToString(); //ex: 0.3434
        string b = cuted.Substring(2, 1); // 3 (string)
        //Debug.Log("life: " + int.Parse(b));
        lifeCount = int.Parse(b); // 3 (int)
    }

    private void generateMainBlok()
    {
        //jumlahOperand = 0; //TODO: sumpah bingung ini variabel kok ada dibuat lagi di difficulty, soalnya hasilnya ga jelas, operand / jumlah blok pasti ga bisa konsisten, kenapa ya?
        jumlahBlok = 0;
        jumlahTambah = 0;
        jumlahKurang = 0;
        jumlahKali = 0;
        jumlahBagi = 0;
        //dibawah diff 4, jumlah balok 3
        //diatas diff 4, jumlsh balok 4. max operator 2
        //diatas diff 6, jumlah balok 5 max operator 3
        Debug.LogWarning("target: " + gl.targetDifficulty);
        if (gl.targetDifficulty == 0f)
        {
            //Debug.Log("Init jumlah balok?");

            jumlahBlok = Random.Range(3, 6);
        }
        else if (gl.targetDifficulty < 0.4f) //46 || EASY
        {
            jumlahBlok = 3;
        }
        else if (gl.targetDifficulty >= 0.4f && gl.targetDifficulty <= 0.6f) //0.46 - 0.65 || MED
        {
            jumlahBlok = 4;
        }
        else if (gl.targetDifficulty > 0.6f) //0.65 || HARD
        {
            jumlahBlok = 5;
        }



        Debug.LogWarning("jumlah blok: " + jumlahBlok);
        int maxOperatorCount;

        // check dulu jumlahblok dapet berapa,
        // jumlah balok biasa harus setngah atau lebih daripada jumlah operator, contoh 3. balok 2 op 1| contoh 4 balok 3 op 1, atau balok 2 op 2
        if (jumlahBlok == 3)
        {
            //if(gl.targetDifficulty <= 0.27) // TODO: ini km perhatiin, ini buat set minimum
            //{
            //    maxOperatorCount = 0;
            //    jumlahTambah = 2;
            //    jumlahKurang = 1;
            //}
            //else
            //{
            //    maxOperatorCount = 1;
            //    jumlahTambah = 1;
            //    jumlahKurang = 1;
            //}
            maxOperatorCount = Random.Range(0, 2);
            if (maxOperatorCount == 1)
            {
                jumlahTambah = 1;
                jumlahKurang = 1;
            }
            else if (maxOperatorCount == 0)
            {
                jumlahTambah = 2;
                jumlahKurang = 1;
            }

            //Debug.Log("jumlah operator: " + maxOperatorCount);
            if (maxOperatorCount == 1)
            {
                int rand = Random.Range(1, 3);
                if (rand == 1)
                {
                    jumlahKali = maxOperatorCount;
                }
                else
                {
                    Debug.LogWarning("Ada Bagi: " + maxOperatorCount);
                    jumlahBagi = maxOperatorCount;
                }
            }

            //Debug.Log("Operand harusnya: " + maxOperatorCount);


        }
        else if (jumlahBlok == 4)
        {
            //maxOperatorCount = Random.Range(1, 2);

            bool isBagiMax = false;
            int rand = 0;
            //if (gl.targetDifficulty <= 0.5) // TODO: ini km perhatiin, masak begini?
            //{
            //    maxOperatorCount = 1;
            //}
            //else
            //{
            //    maxOperatorCount = 2;
            //}
            maxOperatorCount = Random.Range(1, 3); //1-2

            for (int i = 0; i < maxOperatorCount; i++)
            {
                if (!isBagiMax)
                {
                    rand = Random.Range(1, 3); // buat cari ada apakah operator bagi/kali, kalo bagi maximal 1
                }
                else
                {
                    rand = 1;
                }

                if (rand == 1)
                {
                    jumlahKali++;
                }
                else
                {
                    Debug.LogWarning("Ada Bagi 1");
                    
                    jumlahBagi++;
                    isBagiMax = true;
                }
            }

            int jumlahblokkeluar = jumlahBlok - maxOperatorCount; // ini buat tentuin jumlah operator yang diproses apakah kali/bagi nanti termasuk positive apa negative
            jumlahTambah = 1;
            jumlahKurang = 1;
            jumlahblokkeluar = jumlahblokkeluar - 2;
            while (jumlahblokkeluar >= 1) //
            {
                //Debug.Log("Current jumlah blok: " + jumlahblokkeluar);
                int rand1 = Random.Range(1, 3);
                if (rand1 == 1)
                {
                    jumlahTambah++;
                    jumlahblokkeluar--;
                }
                else
                {
                    jumlahKurang++;
                    jumlahblokkeluar--;
                }
            }
            Debug.Log("Operand harusnya: " + maxOperatorCount);
        }
        else if (jumlahBlok == 5 || jumlahBlok == 6)
        {
            maxOperatorCount = Random.Range(1, 4);
            bool isBagiMax = false;
            int rand = 0;
            for (int i = 0; i < maxOperatorCount; i++)
            {
                if (!isBagiMax)
                {
                    rand = Random.Range(1, 3); // asign apakah operator bagi/kali
                }
                else
                {
                    rand = 1;
                }

                if (rand == 1)
                {
                    jumlahKali++;
                }
                else
                {
                    //Debug.LogError("Ada Bagi 1" );
                    jumlahBagi++;
                    isBagiMax = true;
                }
            }

            int jumlahblokkeluar = jumlahBlok - maxOperatorCount;
            jumlahTambah = 1;
            jumlahKurang = 1;
            jumlahblokkeluar = jumlahblokkeluar - 2;
            while (jumlahblokkeluar >= 1)
            {
                //Debug.Log("Current jumlah blok: " + jumlahblokkeluar);
                rand = Random.Range(1, 3);
                if (rand == 1)
                {
                    jumlahTambah++;
                    jumlahblokkeluar--;
                }
                else
                {
                    jumlahKurang++;
                    jumlahblokkeluar--;
                }
            }
            Debug.Log("Operand harusnya: " + maxOperatorCount);
        }
        else if (jumlahBlok == 7 || jumlahBlok == 8)
        {
            maxOperatorCount = Random.Range(1, 5);
            bool isBagiMax = false;
            int rand = 0;
            for (int i = 0; i < maxOperatorCount; i++)
            {
                if (!isBagiMax)
                {
                    rand = Random.Range(1, 3); // asign apakah operator bagi/kali
                }
                else
                {
                    rand = 1;
                }

                if (rand == 1)
                {
                    jumlahKali++;
                }
                else
                {
                    Debug.LogError("bagi maxed");
                    jumlahBagi++;
                    isBagiMax = true;
                }
            }

            int jumlahblokkeluar = jumlahBlok - maxOperatorCount;
            jumlahTambah = 1;
            jumlahKurang = 1;
            jumlahblokkeluar = jumlahblokkeluar - 2;
            while (jumlahblokkeluar >= 1)
            {
                // Debug.Log("Current jumlah blok: " + jumlahblokkeluar);
                rand = Random.Range(1, 2);
                if (rand == 1)
                {
                    jumlahTambah++;
                    jumlahblokkeluar--;
                }
                else
                {
                    jumlahKurang++;
                    jumlahblokkeluar--;
                }
            }
        }

        setJumlahOperand(); //cuman sekedar indikator

        //ATAU MAX OP masing masing 1

        // if 3 op max 1
        // if 4 op max 2
        // if op 6 max 3
        // if op 8 max 4


    }

    private void setJumlahOperand() //dicari yang kali bagi doang
    {
        jumlahOperand = 0;
        //Debug.Log("jum Balok: " + jumlahBlok);
        //Debug.Log("jum Operand: " + jumlahOperand);
        if (jumlahTambah > 0)
        {
            //jumlahOperand++;
        }
        if (jumlahKurang > 0)
        {
            //jumlahOperand++;
        }
        if (jumlahKali > 0)
        {
            jumlahOperand += jumlahKali;
        }
        if (jumlahBagi > 0)
        {
            jumlahOperand += jumlahBagi;
        }
        //Debug.Log("Operand after: " + jumlahOperand);
        //Debug.Log("Jumlah operand: " + jumlahOperand);
    }

    private List<Bilangan> generateSoalBilangan(List<Bilangan> bilangan)
    {
        ListBilangan.Clear();
        for (int i = 0; i < jumlahTambah; i++)
        {
            float randNum = Random.Range(1, 10); // disini randomnya TODO mu
            bilangan.Add(gen.newBilangan("+" + randNum.ToString(), "+"));
            ListBilangan.Add(randNum);
        }
        for (int i = 0; i < jumlahKurang; i++)
        {
            float randNum = Random.Range(1, 10); // disini randomnya
            bilangan.Add(gen.newBilangan("-" + randNum.ToString(), "-"));
            ListBilangan.Add(randNum);
        }
        for (int i = 0; i < jumlahKali; i++)
        {
            float randNum = Random.Range(1, 10); // disini randomnya
            int isPos = Random.Range(1, 3); // get ini positive apa negative wkwkw
            sumKali += randNum;
            ListBilangan.Add(randNum);
            if (isPos == 1)
            {
                bilangan.Add(gen.newBilangan("+" + randNum.ToString(), "*"));
            }
            else
            {
                bilangan.Add(gen.newBilangan("-" + randNum.ToString(), "*"));
            }

        }
        for (int i = 0; i < jumlahBagi; i++) // PEMBAGIAN INI TRICKY SEKALI, jadi 0.00002 bisa
        {
            float randNum = Random.Range(1, 10); // disini randomnya
            int isPos = Random.Range(1, 3); // get ini positive apa negative wkwkw
            ListBilangan.Add(randNum);
            if (isPos == 1)
            {
                bilangan.Add(gen.newBilangan("+" + randNum.ToString(), "/"));
            }
            else
            {
                bilangan.Add(gen.newBilangan("-" + randNum.ToString(), "/"));
            }

        }

        return bilangan;
    }

    public List<Bilangan> BuatSoal()
    {
        List<Bilangan> bilangan = new List<Bilangan>();
        bilangan.Clear();
        bilangan = generateSoalBilangan(bilangan);
        Debug.Log("harusnya count bilangan: " + bilangan.Count);

        return bilangan;
    }

    public List<Node> getLastNodes(Node root, int desiredDeep, List<Node> nodes, int deep)
    {
        desiredDeep -= 1;
        foreach (var nd in root.child)
        {
            if (deep < desiredDeep && nd.child.Count > 0)
            {
                getLastNodes(nd, desiredDeep, nodes, deep + 1);
            }
            else
            {
                nodes.Add(nd);
            }
        }

        return nodes.Where(n => n.listBilangan.Any(b => b.op == "*" || b.op == "/") == false).ToList();
    }

    public Node generateTarget(Node root)
    {

        var lastNodes = getLastNodes(root, jumlahBlok, new List<Node>(), 0); // jumlah balok = total depth yang bakal dicari

        int rnd = Random.Range(0, lastNodes.Count);
        var targetNode = lastNodes[rnd];

        return targetNode;
    }

    private bool notIncludeOp(Node node)
    {
        int opCount = 0;
        foreach (Bilangan bil in node.listBilangan)
        {
            if (bil.op == "*" || bil.op == "/")
            {
                opCount++;
            }
        }

        if (opCount >= 1)
        {
            Debug.LogWarning("ada operator di target...");
            return false;
        }
        else
        {
            return true;
        }
    }

    private void ShuffleIndex(List<int> arrayList)
    {
        for (int i = 0; i < arrayList.Count; i++)
        {
            int temp = arrayList[i];
            int randomIndex = Random.Range(i, arrayList.Count);
            arrayList[i] = arrayList[randomIndex];
            arrayList[randomIndex] = temp;
        }
    }

    private float getCombination(int jumlahBlok)
    {
        return (factorial(jumlahBlok) / (factorial(2) * factorial((jumlahBlok - 2))));
    }

    private int factorial(int a)
    {
        int fact = 1;
        for (int x = 1; x <= a; x++)
        {
            fact *= x;
        }

        return fact;
    }

    private Difficulty getDifficulty(List<Bilangan> bilanganObj)
    {
        int jumBalok = 0;
        int jumOperand = 0;
        //Debug.Log("balok count: " + bilanganObj.Count);
        //get jumlah awal nilai variabel
        for (int i = 0; i < bilanganObj.Count; i++)
        {
            jumBalok++;
            if (bilanganObj[i].op == "*" || bilanganObj[i].op == "/")
            {
                jumOperand++;
            }
        }

        float z1 = 0;
        float z2 = 0;
        float z3 = 0;
        float z4 = 0;

        float pz1 = 0;
        float pz2 = 0;
        float pz3 = 0;
        float pz4 = 0;
        float pDiff = 0;
        float wDiff = 0;


        float totalSum = 0;

        foreach (float c in ListBilangan)
        {
            totalSum += c;
        }
        //Debug.Log(totalSum);
        //Debug.Log(jumlahBlok);

        // last weight(20) = z4[0.3] z3 [0.2] z1,z2 [1]
        pz1 = getCombination(jumBalok) / 10;
        z1 = (getCombination(jumBalok) / 10) * 0.937f; // 10=>5 | 28 => 8

        //Debug.Log("z2: " + (totalSum) / 45f);
        pz2 = (totalSum) / 45f;
        z2 = ((totalSum) / 45f) * 0.312f; // ini masih lebih dari 1 |72=>8|45=>5(9max)
        float maxi = ListBilangan.Max();
        float mini = ListBilangan.Min();
        // Debug.Log("Maxi: " + maxi);
        //Debug.Log("Mini: " + mini);
        Debug.Log("z3: " + (1 - ((maxi - mini) / 9f)));
        pz3 = (1 - ((maxi - mini) / 9f));
        z3 = (1 - ((maxi - mini) / 9f)) * 0.254f; // range cari INI KOK BISA MINUS [pembaginya itu maximal dari nilai 1 balok yang bisa dihasilkan]
                                                  //z4 = (jumlahOperand / 4f) * 0.3f;
        Debug.Log("z4: " + (float)jumOperand / (float)jumBalok);
        pz4 = ((float)jumOperand / (float)jumBalok);
        z4 = ((float)jumOperand / (float)jumBalok) * 0.984f;
        //Debug.Log("jumlahOperand: " + jumOperand);
        //Debug.Log("jumlahBlok: " + jumBalok);


        // Debug.Log((z1) + "-" + z2 + "-" + z3 + "-" + z4); //z1, z4, z2, z3
        // Debug.Log((z1 + z2 + z3 + z4) / 2.191f);
        // Debug.Log("----------------");
        

        // if(sl.jumlahKerjakanSoal == 0)
        // {
        //     curDifficulty = 0.5f;
        // }else{
            
        // }
        curDifficulty = (z1 + z2 + z3 + z4) / 2.487f;
        wDiff = curDifficulty;
        pDiff = pz1 + pz2 + pz3 + pz4;


        //TODO: log ini bukan best candidate
        gl.z1 = (getCombination(jumBalok) / 10);
        gl.z2 = ((totalSum) / 45f);
        gl.z3 = (1 - ((maxi - mini) / 9f));
        gl.z4 = ((float)jumOperand / (float)jumBalok);
        gl.currentSum = totalSum;
        gl.difficulty = curDifficulty; // LOG

        Difficulty thisDif = newDifficulty(pz1, pz2, pz3, pz4, pDiff, wDiff);
        return thisDif;
    }

    // cuman buat check remove multiple
    private static bool ShouldRemove(int i)
    {
        return ((i % 2) == 0);
    }


    public Node getTarget(List<Bilangan> bilangan)
    {
        solusi = "";
        bool isPembagian = false;
        List<int> pembagianIndexes = new List<int>();
        int mainPembagianIndex = 0;
        //check pembagian
        for (int i = 0; i < bilangan.Count; i++)
        {
            string s = bilangan[i].bilangan;
            s.Remove(0, 1);
            int x = int.Parse(s);
            if (bilangan[i].op == "/" && !isPembagian) // buat ambil index pembagian pertama
            {
                isPembagian = true;
                mainPembagianIndex = i;
            }
            else if (x % 2 == 0 && bilangan[i].op != "*") // buat ambil semua bilangna genap yang bukan operator *
            {
                pembagianIndexes.Add(i);
            }
            if (x % 2 != 0 && bilangan[i].op == "/") //buat larang ada pembagian yang ganjil.
            {
                return null;
            }
        }

        string bilGenap = "";
        for (int i = 0; i < pembagianIndexes.Count; i++)
        {
            bilGenap += bilangan[pembagianIndexes[i]].bilangan.ToString() + bilangan[pembagianIndexes[i]].op.ToString() + " | ";
        }



        if (isPembagian) // ini belum di check dia harusnya sama sama genap, pastiin juga operator bagi itu maximal 1 [done saat generate jumlah balok]
        {

            //validate genap > 1
            if (pembagianIndexes.Count <= 1)
            {


                return null;
            }

            // Debug.Log("Bilangan Genap: " + bilGenap);
            //Debug.Log("pembagi: " + bilangan[pembagianIndexes[0]].bilangan + bilangan[pembagianIndexes[0]].op);
            //Debug.Log("Main pembagi: " + bilangan[mainPembagianIndex].bilangan + bilangan[mainPembagianIndex].op);


            //Debug.Log("ADA BAGI " + pembagianIndexes.Count);
            List<Bilangan> bilanganTemp = new List<Bilangan>(bilangan);
            List<Bilangan> tempSumbagi = new List<Bilangan>();

            solusi += "(" + bilanganTemp[pembagianIndexes[0]].bilangan + bilanganTemp[pembagianIndexes[0]].op;
            // Debug.Log("solusi added 1");
            tempSumbagi.Add(bilanganTemp[pembagianIndexes[0]]);
            // Debug.Log("sumbagi added 1");


            //dibawah sini sih salahnya
            solusi += " " + bilanganTemp[mainPembagianIndex].bilangan + bilanganTemp[mainPembagianIndex].op + ") ";
            // Debug.Log("solusi added 2");
            tempSumbagi.Add(bilanganTemp[mainPembagianIndex]);
            //Debug.Log("sumbagi added 2");



            // Debug.Log("Remving all index");
            bilanganTemp.RemoveAll(t => (t.bilangan == bilanganTemp[pembagianIndexes[0]].bilangan && t.op == bilanganTemp[pembagianIndexes[0]].op) || (t.bilangan == bilanganTemp[mainPembagianIndex].bilangan && t.op == bilanganTemp[mainPembagianIndex].op));
            //bilanganTemp.RemoveAt(pembagianIndexes[0]);
            //bilanganTemp.Remove(bilanganTemp[pembagianIndexes[0]]);
            //Debug.Log("temp removed 1");
            //bilanganTemp.RemoveAt(mainPembagianIndex);
            // bilanganTemp.Remove(bilanganTemp[mainPembagianIndex]);
            //Debug.Log("temp removed 2");


            //tempSumbagi.Reverse(0, 2);
            //  Debug.Log(tempSumbagi[0].bilangan + " | " + tempSumbagi[1].bilangan);
            Bilangan newBilBagi = gen.Hitung(tempSumbagi);

            if (newBilBagi.op == "invalid")
            {
                return null;
            }

            bilanganTemp.Add(newBilBagi);
            // Debug.LogWarning(bilanganTemp.Count);

            while (bilanganTemp.Count > 1)
            {
                List<Bilangan> tempSum = new List<Bilangan>();
                //Debug.LogWarning("random 1");
                int rand = Random.Range(0, bilanganTemp.Count); //get random index untuk bilangan kedua dihitung
                solusi += "(" + bilanganTemp[rand].bilangan + bilanganTemp[rand].op;
                tempSum.Add(bilanganTemp[rand]);
                bilanganTemp.RemoveAt(rand);
                // Debug.LogWarning("END random 1");
                // Debug.LogWarning("random 2");
                rand = Random.Range(0, bilanganTemp.Count);
                solusi += " " + bilanganTemp[rand].bilangan + bilanganTemp[rand].op + ") ";
                tempSum.Add(bilanganTemp[rand]);
                bilanganTemp.RemoveAt(rand);
                //Debug.LogWarning("END random 2");


                Bilangan newBil = gen.Hitung(tempSum);
                if (newBil.op == "invalid")
                {
                    return null;
                }
                bilanganTemp.Add(newBil);
            }
            if (bilanganTemp[0].op == "/" || bilanganTemp[0].op == "*") // biar ga ada bilangan akhir itu operator
            {
                return null;
            }

            //buat prevent ada hasil akir yang komanya ga bner ex: 0.333333 . minimal 2 angka dibelakang koma ex: 6.25
            string sHasil = bilanganTemp[0].bilangan.ToString();
            if (sHasil.Contains(".")){
                if(sHasil.Length >= 5)
                {
                    return null;
                }
            }

            

            return gen.newNode(bilanganTemp, null, null);
        }
        else
        {
            //Debug.Log("NONE BAGI");
            //solusi = "";
            List<Bilangan> bilanganTemp = new List<Bilangan>(bilangan);
            // Debug.LogWarning(bilanganTemp.Count);
            while (bilanganTemp.Count > 1)
            {
                List<Bilangan> tempSum = new List<Bilangan>();
                //Debug.LogWarning("random 1");
                int rand = Random.Range(0, bilanganTemp.Count); //get random index untuk bilangan kedua dihitung
                solusi += "(" + bilanganTemp[rand].bilangan + bilanganTemp[rand].op;
                tempSum.Add(bilanganTemp[rand]);
                bilanganTemp.RemoveAt(rand);
                // Debug.LogWarning("END random 1");
                //Debug.LogWarning("random 2");
                rand = Random.Range(0, bilanganTemp.Count);
                solusi += " " + bilanganTemp[rand].bilangan + bilanganTemp[rand].op + ") ";
                tempSum.Add(bilanganTemp[rand]);
                bilanganTemp.RemoveAt(rand);
                //Debug.LogWarning("END random 2");
                //Debug.Log(tempSum[0].bilangan + " | " + tempSum[1].bilangan);
                Bilangan newBil = gen.Hitung(tempSum);
                if (newBil.op == "invalid")
                {
                    return null;
                }
                bilanganTemp.Add(newBil);
                //Debug.LogWarning(bilanganTemp.Count);
            }
            if (bilanganTemp[0].op == "/" || bilanganTemp[0].op == "*") // biar ga ada bilangan akhir itu operator
            {
                return null;
            }
            return gen.newNode(bilanganTemp, null, null);
        }

    }

    public void GenerateSoal(List<Bilangan> bilangan, Node target)
    {

        Debug.LogWarning(solusi);
        gen.generateTarget(target);
        gen.GenerateLife(lifeCount);
        StartCoroutine(gen.instantiateforSec(bilangan, 0.7f));
    }

}
