﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Archivos;

namespace EntidadesInstanciables
{
    [Serializable]
  public  class Jornada
    {
        private List<Alumno> _alumnos;
        private Gimnasio.EClases _clase;
        private Instructor _instructor;

        #region "Metodos"

        private Jornada()
        {
            this._alumnos = new List<Alumno>();
        }
        public Jornada(Gimnasio.EClases clase, Instructor instructor)
            : this()
        {
            this._clase = clase;
            this._instructor = instructor;
        }


        /// <summary>
        /// Verifica si el alumno participa de la clase
        /// </summary>
        /// <param name="j"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        public static bool operator ==(Jornada j, Alumno a)
        {
            if (!Object.ReferenceEquals(j, null) && !Object.ReferenceEquals(a, null))
            {
                foreach (Alumno item in j._alumnos)
                {
                    if (item == a)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// Verifica si el alumno no participa en la clase
        /// </summary>
        /// <param name="j"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        public static bool operator !=(Jornada j, Alumno a)
        {
            return !(j == a);
        }

        /// <summary>
        /// Agrega un alumno a la clase si el mismo no participa de la clase
        /// </summary>
        /// <param name="j"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        public static Jornada operator +(Jornada j, Alumno a)
        {
            if (!Object.ReferenceEquals(j, null) && !Object.ReferenceEquals(a, null))
            {
                if (j != a)
                {
                    j._alumnos.Add(a);
                }
            }
            return j;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("\nJORNADA:");
            sb.AppendLine("");
            sb.AppendFormat("CLASE DE {0} POR NOMBRE COMPLETO {1} \n", this._clase.ToString(), this._instructor.ToString());
            
            sb.AppendFormat("ALUMNOS:");
            foreach (Alumno item in this._alumnos)
            {
                sb.AppendFormat("\n" + item.ToString());
            }
            sb.AppendLine("");
            sb.AppendFormat("<-------------------------->");
            return sb.ToString();
        }

        public static bool Guardar(Jornada jornada)
        {
            Texto text = new Texto();
            if (text.guardar("C:\\Users\\sam\\Desktop.Jornada.xml", jornada.ToString()))
            {
                //Console.WriteLine("El archivo se escribio correctamente");
                return true;
            }
            return false;
        }


        public static bool Leer(Jornada jornada)
        {
            Texto text = new Texto();
            string datos;
            if (text.leer("C:\\Users\\sam\\Desktop.Jornada.xml", out datos))
            {
                Console.WriteLine("El archivo se leyo correctamente");
                Console.WriteLine(datos);
                return true;
            }
            return false;
        }


        #endregion
    }
}
