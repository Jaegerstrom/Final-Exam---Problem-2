Imports System.Data.SqlClient
Imports MySql.Data.MySqlClient

Public Class Form1

    Dim connString As String = "server=localhost; userid=root; password=; database=musicstudio_db; port=3307;"
    Dim conn As MySqlConnection

    ' 1. FORM LOAD
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        conn = New MySqlConnection(connString)
        LoadData()
    End Sub

    ' HELPER FUNCTION
    Private Sub LoadData()
        Try
            If conn.State = ConnectionState.Closed Then conn.Open()

            Dim query As String = "SELECT * FROM tracks_tbl"
            Dim cmd As New MySqlCommand(query, conn)
            Dim adapter As New MySqlDataAdapter(cmd)
            Dim table As New DataTable()

            adapter.Fill(table)
            dgvPreview.DataSource = table

        Catch ex As Exception
            MessageBox.Show("Database Error: " & ex.Message, "Connection Failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Sub

    ' 2. ADD BUTTON
    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        If txtTitle.Text = "" Or txtArtist.Text = "" Or cmbGenre.Text = "" Then
            MessageBox.Show("Please fill in all fields!", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Try
            conn.Open()
            Dim query As String = "INSERT INTO tracks_tbl (title, artist, genre, duration) VALUES (@title, @author, @genre, @duration)"
            Dim cmd As New MySqlCommand(query, conn)

            cmd.Parameters.AddWithValue("@title", txtTitle.Text)
            cmd.Parameters.AddWithValue("@author", txtArtist.Text)
            cmd.Parameters.AddWithValue("@genre", cmbGenre.Text)
            cmd.Parameters.AddWithValue("@duration", txtDuration.Text)

            cmd.ExecuteNonQuery()

            MessageBox.Show("Track added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

            ClearFields()
            LoadData()

        Catch ex As Exception
            MessageBox.Show("Error adding track: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            conn.Close()
        End Try
    End Sub

    ' 3. GRID CLICK
    Private Sub dgvPreview_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvPreview.CellClick
        Try
            If e.RowIndex >= 0 Then
                Dim row As DataGridViewRow = dgvPreview.Rows(e.RowIndex)

                txtTrackID.Text = row.Cells("id").Value.ToString()
                txtTitle.Text = row.Cells("title").Value.ToString()
                txtArtist.Text = row.Cells("artist").Value.ToString()
                cmbGenre.Text = row.Cells("genre").Value.ToString()
                txtDuration.Text = row.Cells("artist").Value.ToString()

            End If
        Catch ex As Exception
        End Try
    End Sub

    ' 4. UPDATE BUTTON
    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        If txtTrackID.Text = "" Then
            MessageBox.Show("Please select a book to update.", "Selection Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Try
            conn.Open()
            Dim query As String = "UPDATE tracks_tbl SET title=@title, author=@artist, category=@genre, duration=@duration WHERE id=@id"
            Dim cmd As New MySqlCommand(query, conn)

            cmd.Parameters.AddWithValue("@id", txtTrackID.Text)
            cmd.Parameters.AddWithValue("@title", txtTitle.Text)
            cmd.Parameters.AddWithValue("@artist", txtArtist.Text)
            cmd.Parameters.AddWithValue("@genre", cmbGenre.Text)
            cmd.Parameters.AddWithValue("@duration", txtDuration.Text)

            cmd.ExecuteNonQuery()

            MessageBox.Show("Record Updated Successfully!")
            ClearFields()
            LoadData()

        Catch ex As Exception
            MessageBox.Show("Update Failed: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            conn.Close()
        End Try
    End Sub

    ' 5. DELETE BUTTON
    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        If txtTrackID.Text = "" Then
            MessageBox.Show("Please select a track to delete.", "Selection Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim result As DialogResult = MessageBox.Show("Are you sure you want to delete this track?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)

        If result = DialogResult.Yes Then
            Try
                conn.Open()
                Dim query As String = "DELETE FROM tracks_tbl WHERE id=@id"
                Dim cmd As New MySqlCommand(query, conn)

                cmd.Parameters.AddWithValue("@id", txtTrackID.Text)

                cmd.ExecuteNonQuery()

                ClearFields()
                LoadData()

            Catch ex As Exception
                MessageBox.Show("Delete Failed: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Finally
                conn.Close()
            End Try
        End If
    End Sub

    ' 6. LOAD DATA BUTTON 
    Private Sub btnLoad_Click(sender As Object, e As EventArgs) Handles btnLoad.Click
        LoadData()
    End Sub

    Private Sub ClearFields()
        txtTrackID.Text = ""
        txtTitle.Text = ""
        txtArtist.Text = ""
        cmbGenre.SelectedIndex = -1
        txtDuration.Text = ""

    End Sub

End Class
