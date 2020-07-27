using UnityEngine;
using UnityEngine.UI;

public class GameControl : MonoBehaviour {

    private static GameObject whoWinsTextShadow, player1MoveText, player2MoveText,p1infectionst,p2infectionst;

    private static GameObject player1, player2;

    public static int diceSideThrown = 0;
    public static int player1StartWaypoint = 0;
    public static int player2StartWaypoint = 0;


    public Transform[] viruses,senitizers,immunities;
    private int[] virusLoc,senLocs,imLocs;
    public static bool gameOver = false;
    private bool p1Infected , p2Infected ,isP1immune,isP2immune;
    int player1MoveCount=0,player2MoveCount=0,player1TurnCounter,player2TurnCounter;



    // Use this for initialization
    void Start () {

        whoWinsTextShadow = GameObject.Find("WhoWinsText");
        player1MoveText = GameObject.Find("Player1MoveText");
        player2MoveText = GameObject.Find("Player2MoveText");
        p1infectionst = GameObject.Find("Player1InfectionStatus");
        p2infectionst = GameObject.Find("Player2InfectionStatus");

       // p1infectionst.GetComponent<Text>().text = "changed";
        
        player1 = GameObject.Find("Player1");
        player2 = GameObject.Find("Player2");
        player1TurnCounter=-1;
        player2TurnCounter=-1;
        isP1immune=false;
        isP2immune=false;
        p1Infected=false;
        p2Infected=false;
        player1.GetComponent<FollowThePath>().moveAllowed = false;
        player2.GetComponent<FollowThePath>().moveAllowed = false;
        randomGen();
        randomGenS();
        randomGenI();
        whoWinsTextShadow.gameObject.SetActive(false);
        player1MoveText.gameObject.SetActive(true);
        player2MoveText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(p1Infected)
        {
            p1infectionst.GetComponent<Text>().text = "p1 infected and turn left "+player1TurnCounter;
        }
        else
        {
            p1infectionst.GetComponent<Text>().text = "p1 is healthy ";
        }

        if(p2Infected)
        {
            p2infectionst.GetComponent<Text>().text = "p1 infected and turn left "+player2TurnCounter;
        }
        else
        {
            p2infectionst.GetComponent<Text>().text = "p2 is healthy ";
        }
        if(isInfectedZone(player1MoveCount)&&!p1Infected)
        {
            if(isP1immune)
            {
                isP1immune=false;
                Debug.Log("player 1 lost immunity");
            }
            else
            {
                player1TurnCounter=5;
                p1Infected = true;
            }
            
            
            Debug.Log("player 1 entered infected zone");
        }
        if(player1MoveCount==player2MoveCount||Mathf.Abs(player1MoveCount-player2MoveCount)==1)
        {
           // Debug.Log("p1 and p2 are on close contact");
            if(p1Infected&&!p2Infected)
            {
                Debug.Log("p2 got infected");
                player2TurnCounter=5;
                p2Infected=true;
            }else if(p2Infected&&!p1Infected)
            {
                Debug.Log("p1 got infected");
                player1TurnCounter=5;
                p1Infected=true;
            }
            else if(p1Infected&&p2Infected)
            {
                Debug.Log("p2 and p1 are already infected");
            }
            else{
              //  Debug.Log("neither got infected");
            }
        }
        if(issenizerzone(player1MoveCount)&&p1Infected)
        {
            Debug.Log("p1 senitized");
            Debug.Log(p1Infected);
            player1TurnCounter=-1;
            p1Infected = false;
        }
        if(isImmunityZone(player1MoveCount)&&!isP1immune)
        {
            Debug.Log("p1 gained immunity");
            isP1immune = true;
        }
        if(issenizerzone(player2MoveCount)&&p2Infected)
        {
            Debug.Log("p2 Senitized");
            player2TurnCounter=-1;
            p2Infected = false;
        }
        if(isImmunityZone(player2MoveCount)&&!isP2immune)
        {
            Debug.Log("p2 gained immunity");
            isP2immune = true;
        }
        if(player1TurnCounter==0)
            {
            player1.GetComponent<FollowThePath>().moveAllowed = true;  
            while(player1.GetComponent<Transform>().transform.position!=player1.GetComponent<FollowThePath>().waypoints[0].transform.position)
            {
                player1.GetComponent<Transform>().transform.position = Vector2.MoveTowards(player1.GetComponent<Transform>().transform.position ,
            player1.GetComponent<FollowThePath>().waypoints[0].transform.position,
            1f * Time.deltaTime);
            player1TurnCounter=-1;
            }
            
        
            player1.GetComponent<FollowThePath>().waypointIndex=0;
            player1StartWaypoint=0;
            player1MoveCount=0;
            p1Infected=false;
            player1.GetComponent<FollowThePath>().moveAllowed = false;
            }
        
        
        if (player1MoveCount == 0 && diceSideThrown <=3)
            player1.GetComponent<FollowThePath>().moveAllowed = false;
        else if (player1StartWaypoint + diceSideThrown > 99)
        {
            player1.GetComponent<FollowThePath>().moveAllowed = false;
            Debug.Log("You need a smaller no. to win");
        }
        else
        {
        if (player1.GetComponent<FollowThePath>().waypointIndex > 
            player1StartWaypoint + diceSideThrown)
        {   
            
            player1.GetComponent<FollowThePath>().moveAllowed = false;
            player1MoveText.gameObject.SetActive(false);
            player2MoveText.gameObject.SetActive(true);
            player1MoveCount+=diceSideThrown;
            player1StartWaypoint = player1.GetComponent<FollowThePath>().waypointIndex - 1;
            if(player1TurnCounter!=-1)
            {
               player1TurnCounter--; 
               Debug.Log("p1 turn ="+player1TurnCounter);
            }
        }
        }
        if(isInfectedZone(player2MoveCount)&&!p2Infected)
        {
            if(isP2immune)
            {
                isP2immune=false;
                Debug.Log("player 2 lost immunity");
            }
            else
            {
                player2TurnCounter=5;
                p2Infected=true;
            }
            
            
            Debug.Log("PLAYER 2 ENTERED INFECTED ZONE");
        }

       if(player2TurnCounter==0)
            {
             
              player2.GetComponent<FollowThePath>().moveAllowed = true;  
            while(player2.GetComponent<Transform>().transform.position!=player2.GetComponent<FollowThePath>().waypoints[0].transform.position) 
            {
            player2.GetComponent<Transform>().transform.position = Vector2.MoveTowards(player2.GetComponent<Transform>().transform.position ,
            player2.GetComponent<FollowThePath>().waypoints[0].transform.position,
            1f * Time.deltaTime);
             
            }   
            
            player2.GetComponent<FollowThePath>().waypointIndex=0;
            player2StartWaypoint=0;
            player2MoveCount=0;
            player2TurnCounter=-1;
            player2.GetComponent<FollowThePath>().moveAllowed = false;
            p2Infected=false;
            }
        
        if (player2MoveCount == 0 && diceSideThrown <=3)
            player2.GetComponent<FollowThePath>().moveAllowed = false;
        else if (player2StartWaypoint + diceSideThrown > 99)
        {
            player2.GetComponent<FollowThePath>().moveAllowed = false;
            Debug.Log("You need a smaller no. to win");
        }
        else
        {
        if (player2.GetComponent<FollowThePath>().waypointIndex >
            player2StartWaypoint + diceSideThrown)
        {
            player2.GetComponent<FollowThePath>().moveAllowed = false;
            player2MoveText.gameObject.SetActive(false);
            player1MoveText.gameObject.SetActive(true);
            player2MoveCount+=diceSideThrown;
            player2StartWaypoint = player2.GetComponent<FollowThePath>().waypointIndex - 1;
            if(player2TurnCounter>=0)
            {
                player2TurnCounter--;
                Debug.Log("p2 turn ="+player2TurnCounter);
            }

               
        }
        }
        if (player1.GetComponent<FollowThePath>().waypointIndex == 
            player1.GetComponent<FollowThePath>().waypoints.Length)
        {
            whoWinsTextShadow.gameObject.SetActive(true);
            whoWinsTextShadow.GetComponent<Text>().text = "Player 1 Wins";
            gameOver = true;
        }

        if (player2.GetComponent<FollowThePath>().waypointIndex ==
            player2.GetComponent<FollowThePath>().waypoints.Length)
        {
            whoWinsTextShadow.gameObject.SetActive(true);
            player1MoveText.gameObject.SetActive(false);
            player2MoveText.gameObject.SetActive(false);
            whoWinsTextShadow.GetComponent<Text>().text = "Player 2 Wins";
            gameOver = true;
        }
    }
    void randomGen()
    {
        System.Random r = new System.Random();
        virusLoc = new int[viruses.Length];
        for(int i=0 ; i<viruses.Length;i++)
        {
            virusLoc[i] = r.Next(10,99);
          //  Debug.Log(virusLoc[i]);
            viruses[i].position = player1.GetComponent<FollowThePath>().waypoints[virusLoc[i]].transform.position;
        }
    }
    void randomGenS()
    {
        System.Random r = new System.Random(7);
        senLocs = new int[senitizers.Length];
        int i=0;
        LOOP: while(i<senitizers.Length)
        {

            senLocs[i] = r.Next(10,99);
         //   Debug.Log(virusLoc[i]);
            foreach (var item in virusLoc)
            {
                if(item==senLocs[i])
                {
                    goto LOOP;
                }
            }
            senitizers[i].position = player1.GetComponent<FollowThePath>().waypoints[senLocs[i]].transform.position;
            i++;
        }
    }
    void randomGenI()
    {
        System.Random r = new System.Random(3);
        imLocs = new int[immunities.Length];
        int i=0;
        LOOP: while(i<immunities.Length)
        {
            imLocs[i] = r.Next(10,99);
         //   Debug.Log(virusLoc[i]);
         foreach (var item in virusLoc)
            {
                if(item==imLocs[i])
                {
                    goto LOOP;
                }
            }
            immunities[i].position = player1.GetComponent<FollowThePath>().waypoints[imLocs[i]].transform.position;
            i++;
        }
    }
    public static void MovePlayer(int playerToMove)
    {
        switch (playerToMove) { 
            case 1:
                player1.GetComponent<FollowThePath>().moveAllowed = true;
                break;

            case 2:
                player2.GetComponent<FollowThePath>().moveAllowed = true;
                break;
        }
    }
    bool isInfectedZone(int moveCount)
    {
        foreach (var virusLo in virusLoc)
        {
            if(virusLo==moveCount)
            return true;
        }
        

        return false;
    }
    bool issenizerzone(int moveCount)
    {
        foreach (var virusLo in senLocs)
        {
            if(virusLo==moveCount)
            return true;
        }
        return false;
    }
    bool isImmunityZone(int moveCount)
    {
        foreach (var virusLo in imLocs)
        {
            if(virusLo==moveCount)
            return true;
        }
        return false;
    }
}