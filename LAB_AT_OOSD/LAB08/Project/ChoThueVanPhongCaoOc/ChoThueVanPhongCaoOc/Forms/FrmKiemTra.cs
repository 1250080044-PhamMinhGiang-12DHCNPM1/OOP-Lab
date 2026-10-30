using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using ChoThueVanPhongCaoOc.Data;

namespace ChoThueVanPhongCaoOc.Forms
{
    internal sealed class FrmKiemTra : Form
    {
        internal FrmKiemTra(){Text="Kiểm tra phòng và xử phạt";Width=1000;Height=560;StartPosition=FormStartPosition.CenterParent;Controls.Add(new CrudPanel("SELECT k.*,p.LyDo,p.SoTien FROM PhieuKiemTra k LEFT JOIN PhieuXuPhat p ON p.MaKT=k.MaKT ORDER BY k.NgayKT DESC","INSERT PhieuKiemTra(MaPhong,NgayKT,TinhTrang,KetLuan) VALUES(@MaPhong,@NgayKT,@TinhTrang,@KetLuan)",new FieldSpec("Mã phòng","@MaPhong"),new FieldSpec("Ngày KT","@NgayKT",FieldKind.Date),new FieldSpec("Tình trạng","@TinhTrang"),new FieldSpec("Kết luận","@KetLuan")));
            var phat=new Button{Text="Lập phiếu phạt cho lần kiểm tra mới nhất",Dock=DockStyle.Bottom,Height=36};phat.Click+=(s,e)=>LapPhat();Controls.Add(phat);}
        private void LapPhat(){using(var f=new Form{Text="Lập phiếu phạt",Width=390,Height=190,StartPosition=FormStartPosition.CenterParent}){var ly=new TextBox{Left=110,Top=20,Width=240};var tien=new NumericUpDown{Left=110,Top=55,Width=160,Maximum=1000000000,ThousandsSeparator=true};var b=new Button{Text="Lưu",Left=110,Top=95};f.Controls.AddRange(new Control[]{new Label{Text="Lý do",Left=15,Top=24},ly,new Label{Text="Số tiền",Left=15,Top=59},tien,b});b.Click+=(s,e)=>{try{Db.Execute("INSERT PhieuXuPhat(MaKT,LyDo,SoTien) SELECT TOP 1 MaKT,@LyDo,@SoTien FROM PhieuKiemTra ORDER BY MaKT DESC",new SqlParameter("@LyDo",ly.Text.Trim()),new SqlParameter("@SoTien",tien.Value));f.DialogResult=DialogResult.OK;}catch(Exception ex){MessageBox.Show(ex.Message);}};f.ShowDialog(this);}}
    }
}
