using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Security.Principal;

namespace Prueba.Modelos
{
    public class ApiModel
    {
        public string Name { get; set; }

        public Estimated_Diameter Estimated_Diameter { get; set; }

        public bool Is_potentially_hazardous_asteroid { get; set; }

        public List<Element> Close_approach_data { get; set; }
    }



    public class Element {
        public DateTime Close_approach_date { get; set; }
        public Relative_velocity Relative_velocity { get; set; }

        public string Orbiting_body { get; set; }

    }

    public class Relative_velocity {
        public float Kilometers_per_second { get; set; }

        public float Kilometers_per_hour { get; set; }
        public float Miles_per_hour { get; set; }
    }


    public class Estimated_Diameter {
        public  Kilometers Kilometers { get; set; }
        public Meters Meters { get; set; }
        public Miles Miles { get; set; }
        public Feet Feet { get; set; }
    }

    public class Kilometers {
        public float Estimated_diameter_min { get; set; }
        public float Estimated_diameter_max { get; set; }
    }

    public class Meters {
        public float Estimated_diameter_min { get; set; }
        public float Estimated_diameter_max { get; set; }
    }

    public class Miles {
        public float Estimated_diameter_min { get; set; }
        public float Estimated_diameter_max { get; set; }
    }

    public class Feet {
        public float Estimated_diameter_min { get; set; }
        public float Estimated_diameter_max { get; set; }
    }

}
