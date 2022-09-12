using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CheckMethod//เอาระยะหรือเข้าพื้นที่
{
    Distance,
    Trigger
}
public class Showinvisible : MonoBehaviour
{
    public Transform player;//ตำแหน่งผูเล่น
    public CheckMethod checkMethod;//เลือกว่าจะใช้ระยะทางหรือเข้าพื้นที่
    public float loadRange;//ระยะกำหนดระหว่างตัวที่จะโชว์กับผู้เล่น

    private bool isLoaded;//เช็คว่าโหลดไหม
    private bool shouldLoad;//ควรโหลดไหม

    //add
    public MeshRenderer renderers;//ไปเอาโค้ดเรนเดอร์เรอร์
    public LighterSystem lighter;//ไปเอาโค้ดLighter
    public Collider collider;//ไปเอาโค้ดCollider

    public float t = 5.0f;//เวลา
    public float speed = .5f;//ความเร็ว

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("PlayerArmature").transform;//ไปget ตำแหน่งผู้เล่น        
        renderers = GetComponent<MeshRenderer>();//ไปget meshrenderของตัวที่ใส่
        lighter = GameObject.FindGameObjectWithTag("Lamp").GetComponent<LighterSystem>();//ไปget โค้ดที่ Tag Lamp 
        collider = GetComponent<Collider>();//ไปget Colliderของตัวที่ใส่
                                            
    }

    // Update is called once per frame
    void Update()
    {
        //Material[] mats = renderers.materials;

        if (lighter.openlamb == true)//ถ้าเปิดตะเกียง
        {
            Debug.Log(Mathf.Sin(t * speed));
            collider.enabled = true;//เปิดcollider
            if (checkMethod == CheckMethod.Distance)//เลือกระยะทาง
            {
                DistanceCheck();//เช็คระยะห่าง
            }
            else if (checkMethod == CheckMethod.Trigger)//เลือกพื้นที่
            {
                TriggerCheck();//เช็คว่าเข้าไหม
            }
        }
        else//ถ้าปิดตะเกียง
        {
            collider.enabled = false;//ปิดcollider
        }
        
    }

    

    void DistanceCheck()//เช็คระยะห่าง
    {
        //Checking if the player is within the range
        if (Vector3.Distance(player.position, transform.position) < loadRange)//ระยะห่างผู้เล่นน้อยหว่าจุดที่กำหนดไหม
        {
            if (!isLoaded)//ถ้าเท่ากับจริงให้เข้าเงื่อนไข
            {
                LoadScene();//โหลก
            }
        }
        else
        {
            UnLoadScene();//ไม่โหลด
        }
    }
    void TriggerCheck()//เช็คว่าเข้าไหม
    {
        //shouldLoad is set from the Trigger methods
        if (shouldLoad)//ควรโหลด
        {
            LoadScene();//โหลก
        }
        else
        {
            UnLoadScene();//ไม่โหลด
        }
    }
    void LoadScene()
    {
        Material[] mats = renderers.materials;//ประกาศค่า mats
        renderers.gameObject.SetActive(true);//เปิดกันไว้ก่อน        
        mats[0].SetFloat("_Cutoff", Mathf.Sin(t * speed));//Setตัวสลาย
        t += Time.deltaTime;//เวลาเพิ่มขึ้น
        if(Mathf.Sin(t*speed) <= 0)//ถ้าคำนวณน้อยกว่า0
        { mats[0].SetFloat("_Cutoff", 0); }//Setเป็น0        
        renderers.material = mats[0];//***สำคัญ ต้องประกาศว่า  renderers.material = mats[0]***ไม่งั้นไม่ขึ้น      
        isLoaded = true;        
    }

    void UnLoadScene()
    {
        if (isLoaded)
        {
            Material[] mats = renderers.materials;
            renderers.gameObject.SetActive(true);
            mats[0].SetFloat("_Cutoff", Mathf.Sin(t * speed));
            t += Time.deltaTime;
            if (Mathf.Sin(t * speed) >= 0.9f) { mats[0].SetFloat("_Cutoff", 1); }
            renderers.material = mats[0];
            isLoaded = false;
        }
    }

    private void OnTriggerEnter(Collider other)//เช็คTrigger
    {
        if (other.CompareTag("Player"))
        {
            shouldLoad = true;
        }
    }

    private void OnTriggerExit(Collider other)//เช็คTrigger
    {
        if (other.CompareTag("Player"))
        {
            shouldLoad = false;
        }
    }
}
