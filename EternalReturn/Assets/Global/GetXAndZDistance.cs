using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class ExceptY
{
    public static Vector3 ExceptYPos(Vector3 position_)
    {
        Vector3 exceptYPos = new Vector3(position_.x, 0, position_.z);
        return exceptYPos;
    }

    public static float ExceptYDistance(Vector3 startPos_, Vector3 endPos_)
    {
        float distance_ = Vector3.Distance(
            new Vector3(startPos_.x, 0, startPos_.z),
            new Vector3(endPos_.x, 0, endPos_.z));

        return distance_;
    }
}
