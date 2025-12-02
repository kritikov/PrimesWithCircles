//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace PrimesWithCircles.ViewModels
//{
//    using global::PrimesWithCircles.Models.PrimesWithCircles.Models;
//    using System.Collections.ObjectModel;

//    namespace PrimesWithCircles.Models
//    {
//        public class MainViewModel
//        {
//            public ObservableCollection<CircleModel> Circles { get; } = new();
//            public int GlobalLapCounter { get; private set; } = 2;

//            public double BaseAngularSpeed = Math.PI; // αλλάζει από slider

//            public MainViewModel()
//            {
//                AddCircle(1);
//                AddCircle(2);
//            }

//            public CircleModel AddCircle(int number)
//            {
//                var circle = new CircleModel(number, BaseAngularSpeed);
//                Circles.Add(circle);
//                return circle;
//            }

//            public void Reset()
//            {
//                Circles.Clear();
//                GlobalLapCounter = 2;

//                AddCircle(1);
//                AddCircle(2);
//            }

//            public void IncrementGlobalLap()
//            {
//                GlobalLapCounter++;
//            }
//        }
//    }

//}
