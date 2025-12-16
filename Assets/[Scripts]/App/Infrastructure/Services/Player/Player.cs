using System;
using UnityEngine;

namespace Serjbal
{
    public class Player : MonoBehaviour, IPlayer
    {
        public Action OnMow { get; set; }
        public Action OnUpgrade { get; set; }
        public Action OnSell { get; set; }
    }

    public interface IPlayer : IService
    {
        Action OnMow { get; set; }
        Action OnUpgrade { get; set; }
        Action OnSell { get; set; }
    }
}
