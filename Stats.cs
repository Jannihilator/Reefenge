using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    // Start is called before the first frame update
    public class Ship{
        public int health;
        public int damage;
        public float atkSpeed;
        public float range;
        public float bulletSpeed;
        public float speed;
        public Ship(int _health, int _damage, float _atkSpeed, float _range, float _bulletSpeed, float _speed)
        {
            health = _health;
            damage= _damage;
            atkSpeed = _atkSpeed;
            range = _range;
            bulletSpeed = _bulletSpeed;
            speed = _speed;
        }
    }

    public Ship normalShip;
    public Ship tankShip;
    public Ship fastShip;
    void Start(){
        normalShip = new Ship(40, 5, 1.5f, 7, 13, 4);
        tankShip = new Ship(120, 8, 1.2f, 5, 2.5f, 2);
        fastShip = new Ship(15, 2, 0.7f, 2.5f, 6, 8);
    }
}
