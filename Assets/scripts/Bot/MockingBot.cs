using UnityEngine;
using System.Collections.Generic;

public class MockingBot : Bot {

    MockingBotPart part;
    List<Unit> mockingparts;
    bool On;
    
    Vector2 position;
    public void init(MockingBotPart part, List<Unit>[,] units)
    {
        this.part = part;
        mockingparts = new List<Unit>();
        int count = 0;
        Vector2 initpos = new Vector2(transform.position.x, transform.position.y - 1);
        for(int i=0; i<3; i++)
        {
            for(int j=0; j<3; j++)
            {
                Vector2 temppos = new Vector2(initpos.x + i, initpos.y - j); ;
                if (temppos.x == leanable.transform.position.x && temppos.y == leanable.transform.position.y)
                {
                    mockingparts.Add(leanable);
                    continue;
                }
                gameObject.AddComponent<MockingBotPart>();
                MockingBotPart temp = GetComponents<MockingBotPart>()[count];
                temp.position = temppos;
                temp.api = part.api;
                units[(int)temp.position.x, (int)temp.position.y].Add(temp);
                mockingparts.Add(temp);
                count++;
            }
        }
        //mock = true;
        position = new Vector2(initpos.x + 1, initpos.y + 1);
    }
    
    void Update()
    {
        if (On)
        {
            if (player.position.y < transform.position.y)
                Move();
        }
    }

    public override void TurnOn()
    {
        int count = 0;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                mockingparts[count].api.RemoveFromDatabase(mockingparts[count]);
                mockingparts[count].position = new Vector2(mockingparts[count].position.x, mockingparts[count].position.y + 1);
                mockingparts[count].api.AddToDatabase(mockingparts[count]);
                count++;
            }
        }
        transform.GetChild(0).transform.position = new Vector3(transform.GetChild(0).transform.position.x, transform.GetChild(0).transform.position.y + 1, transform.GetChild(0).transform.position.z);
        On = true;
    }

    public void Move()
    {
        if (player.position.x != position.x)
        {
            if (transform.position.x + 1 > player.position.x)
            {
                if (!CanMove(Direction.Left))
                    return;
                transform.position = new Vector3(transform.position.x - 1, transform.position.y, transform.position.z);
                position = new Vector2(position.x - 1, position.y);
            }
            else if(transform.position.x + 1 < player.position.x)
            {
                if (!CanMove(Direction.Right))
                    return;
                transform.position = new Vector3(transform.position.x + 1, transform.position.y, transform.position.z);
                position = new Vector2(position.x + 1, position.y);
            }
            int count = 0;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    mockingparts[count].api.RemoveFromDatabase(mockingparts[count]);
                    mockingparts[count].position = new Vector2(transform.position.x + i, transform.position.y - j);
                    mockingparts[count].api.AddToDatabase(mockingparts[count]);
                    count++;
                }
            }
            CheckFall();
        }
    }

    public bool CanMove(Direction dir)
    {
        Vector2 temppos;
        if (dir == Direction.Right)
        {
            for(int i=0; i<3; i++)
            {
                temppos = new Vector2(position.x + 2, position.y - i);
                if (!Toolkit.IsEmpty(temppos))
                    return false;
            }
        }
        else if(dir == Direction.Left)
        {
            for (int i = 0; i < 3; i++)
            {
                temppos = new Vector2(position.x - 2, position.y - i);
                if (!Toolkit.IsEmpty(temppos))
                    return false;
            }
        }

        return true;
    }

    public void CheckFall()
    {
        Vector2 rightlegpos = new Vector2(position.x + 1, position.y - 3);
        Vector2 leftlegpos = new Vector2(position.x - 1, position.y - 3);
        if(Toolkit.IsEmpty(Toolkit.VectorSum(rightlegpos, Direction.Down)))
        {
            RightFall();
        }
        else if (Toolkit.IsEmpty(Toolkit.VectorSum(leftlegpos, Direction.Down)))
        {
            LeftFall();
        }
    }

    public void RightFall()
    {
        Debug.Log("Right fall");
        position = new Vector2(position.x + 2, position.y - 4);
        transform.position = new Vector3(transform.position.x + 4, transform.position.y - 6, transform.position.z);
        Debug.Log(position);
        int count = 0;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                mockingparts[count].api.RemoveFromDatabase(mockingparts[count]);
                mockingparts[count].position = new Vector2(position.x - 1 + i, position.y - j);
                mockingparts[count].api.AddToDatabase(mockingparts[count]);
                count++;
            }
        }
        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 180);
        On = false;
    }


    public void LeftFall()
    {
        Debug.Log("Left fall");
        position = new Vector2(position.x - 2, position.y - 4);
        transform.position = new Vector3(transform.position.x , transform.position.y - 6, transform.position.z);
        Debug.Log(position);
        int count = 0;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                mockingparts[count].api.RemoveFromDatabase(mockingparts[count]);
                mockingparts[count].position = new Vector2(position.x - 1 + i, position.y - j);
                mockingparts[count].api.AddToDatabase(mockingparts[count]);
                count++;
            }
        }
        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 180);
        On = false;
    }
}
